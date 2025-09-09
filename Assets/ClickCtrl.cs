using UnityEngine;

public class ClickCtrl : MonoBehaviour
{
    [SerializeField] private LayerMask whatIsBubble;
    [SerializeField] private LayerMask whatIsClickable;
    [SerializeField] private float maxDist;

    private ClickBubbleBehavior lastBubble = null;
    private ClickableObject lastClickable = null;

    void Update()
    {
        RaycastHit hit;

        if (Input.GetKey(KeyCode.Mouse0))
        {
            Physics.Raycast(transform.position, transform.forward, out hit, maxDist, whatIsBubble);
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

        hit = new RaycastHit();
        Physics.Raycast(transform.position, transform.forward, out hit, maxDist, whatIsClickable);
        if (hit.collider)
        {
            ClickableObject res = hit.collider.gameObject.GetComponent<ClickableObject>();
            if (res != lastClickable)  // New bubble
            {
                if (lastClickable) lastClickable.CancelHighlight();
                res.Highlight();
            }
            lastClickable = res;
            
            if (Input.GetMouseButtonDown(0))
            {
                res.Interact();
            }
            else
            {
                res.Highlight();

            }
        }
        else
        {
            if (lastClickable) lastClickable.CancelHighlight();
        }
    }
}
