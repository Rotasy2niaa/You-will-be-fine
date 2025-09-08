using UnityEngine;

public class ClickCtrl : MonoBehaviour
{
    [SerializeField] private LayerMask whatIsClickable;
    [SerializeField] private float maxDist;

    void Update()
    {
        if (Input.GetKey(KeyCode.Mouse0))
        {
            RaycastHit hit;
            Physics.Raycast(transform.position, transform.forward, out hit, maxDist, whatIsClickable);
            if (hit.collider)
            {
                hit.collider.gameObject.GetComponent<ClickBubbleBehavior>().OnClicked();
            }
        }
    }
}
