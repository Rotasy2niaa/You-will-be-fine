using UnityEngine;

public class ClickCtrl : MonoBehaviour
{
    [SerializeField] private LayerMask whatIsClickable;
    [SerializeField] private float maxDist;

    private ClickBubbleBehavior lastBubble = null;

    void Update()
    {
        if (Input.GetKey(KeyCode.Mouse0))
        {
            RaycastHit hit;
            Physics.Raycast(transform.position, transform.forward, out hit, maxDist, whatIsClickable);
            if (hit.collider)
            {
                ClickBubbleBehavior res = hit.collider.gameObject.GetComponent<ClickBubbleBehavior>();
                if (res != lastBubble)  // New bubble
                {
                    if (lastBubble) lastBubble.OnReleased();
                    res.OnClicked();
                }
                lastBubble = res;
            }
        }
        else
        {
            if (lastBubble) lastBubble.OnReleased();
            lastBubble = null;
        }
    }
}
