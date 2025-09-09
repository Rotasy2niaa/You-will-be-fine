using UnityEngine;

public class ClickBubbleBehavior : MonoBehaviour
{
    [SerializeField, Min(0)] private float desiredScreenRadius = 40f;
    [SerializeField] private float radiusPulseAmp = 10f;
    [SerializeField] private float targetPulseSpeed;

    private bool progressing = false;
    private float startTime = 0f;
    private float progress = 0f;

    public BubblePhase bubblePhase;

    private void FixedUpdate()
    {
        if (progressing)
        {
            progress = (Time.time - startTime) * targetPulseSpeed;
            if (progress > 2f * Mathf.PI)
            {
                progressing = false;
                EffectManager._instance.SuccessfullySyncPulse();
                bubblePhase.CountBubble();
                Destroy(gameObject);
            }
        }

        // Distance z along camera forward (positive in front)
        Vector3 camToObj = transform.position - Camera.main.transform.position;

        float z = Vector3.Dot(Camera.main.transform.forward, camToObj);

        if (z <= Mathf.Epsilon)
        {
            // Behind camera or too close
            return;
        }

        float desiredWorldRadius;

        // perspective: focal length in pixels
        float f = 0.5f * Screen.height / Mathf.Tan(0.5f * Camera.main.fieldOfView * Mathf.Deg2Rad);
        desiredWorldRadius = (desiredScreenRadius + radiusPulseAmp * Mathf.Sin(progress)) * z / f;

        if (camToObj.magnitude - desiredWorldRadius < Mathf.Epsilon)
        {
            transform.localScale = Vector3.zero;
            return;
        }

        ///////////////////////////////////////////////////////////////////////
        ////////// IMPORTANT: Bubble should not be a child transform //////////
        ///////////////////////////////////////////////////////////////////////
        //// If parent has scale, we must compute local scale so world radius == desiredWorldRadius
        //Vector3 parentLossy = Vector3.one;
        //if (transform.parent != null) parentLossy = transform.parent.lossyScale;

        //// We want uniform scale; pick the max parent scale component to be safe
        //float parentMax = Mathf.Max(Mathf.Abs(parentLossy.x), Mathf.Abs(parentLossy.y), Mathf.Abs(parentLossy.z));
        //if (parentMax <= 0f) parentMax = 1f;

        //// localScaleFactor so: meshLocalRadius * localScaleFactor * parentMax == desiredWorldRadius
        //float localScaleFactor = (meshLocalRadius > 0f) ? (desiredWorldRadius / (meshLocalRadius * parentMax)) : 1f;

        // ensure uniform scale (sphere)
        transform.localScale = new Vector3(desiredWorldRadius, desiredWorldRadius, desiredWorldRadius);
    }

    public void OnClicked()
    {
        startTime = Time.time;

        progressing = true;

        EffectManager._instance.ForceSyncPulse(targetPulseSpeed);
    }

    public void OnReleased()
    {
        startTime = 0f;
        progress = 0f;

        progressing = false;

        EffectManager._instance.EndSyncPulse();
    }
}
