using UnityEngine;

public class ClickableObject : MonoBehaviour
{
    [Header("½×¶Î¿ØÖÆ")]
    [Tooltip("¸ÃÎïÌå´ÓµÚ¼¸ÂÖ¿ªÊ¼¿Éµã»÷")]
    public int requiredStage = 1;   // med Éè³É 3

    [Header("Íâ¹Û")]
    //private Renderer rend;
    //private Color originalColor;
    //public Color hoverColor = new Color(1f, 0.5f, 0.5f);

    [Header("ÃèÊö¶Ô»°")]
    [TextArea] public string[] descriptionLines;

    private bool consumed = false;

    void Start()
    {
        //rend = GetComponent<Renderer>();
        //if (rend) originalColor = rend.material.color;
        transform.Find("outline").gameObject.SetActive(false);
    }

    bool Unlocked()
    {
        var dm = DialogueManager.Instance;
        if (dm == null)
        {
            Debug.LogWarning($"[ClickableObject] {name}: DialogueManager instance is null → returning true by default");
            return true; // or false, depending on your design
        }

        bool result = dm.CurrentStage >= requiredStage;
        Debug.Log($"[ClickableObject] {name}: CurrentStage = {dm.CurrentStage}, RequiredStage = {requiredStage} → Unlocked = {result}");
        return result;
    }


    //void OnMouseEnter()
    //{
    //    Debug.Log(transform.name);
    //    if (consumed) return;
    //    if (!Unlocked()) return;          // Î´µ½½×¶Î£¬²»¸ßÁÁ
    //    //if (rend) rend.material.color = hoverColor;
    //    transform.Find("outline").gameObject.SetActive(true);
    //}

    //void OnMouseExit()
    //{
    //    if (consumed) return;
    //    if (!Unlocked()) return;          // Î´µ½½×¶Î£¬²»±ä»¯
    //    //if (rend) rend.material.color = originalColor;
    //    transform.Find("outline").gameObject.SetActive(false);
    //}

    //void OnMouseDown()
    //{
    //    if (consumed) return;
    //    if (!Unlocked()) return;          // Î´µ½½×¶Î£¬²»¿Éµã

    //    consumed = true;
    //    //if (rend) rend.material.color = originalColor;
    //    transform.Find("outline").gameObject.SetActive(false);

    //    if (descriptionLines != null && descriptionLines.Length > 0)
    //        DialogueManager.Instance?.StartDialogue(descriptionLines);

    //    // TODO: ÕâÀïÕÕ¾É×öÄãµÄ¼ÆÊý£ºclickPhase.RegisterClick();
    //}

    public void Highlight()
    {
        if (consumed) return;
        if (!Unlocked()) return;
        transform.Find("outline").gameObject.SetActive(true);
    }

    public void CancelHighlight()
    {
        if (consumed) return;
        if (!Unlocked()) return;
        transform.Find("outline").gameObject.SetActive(false);
    }

    public void Interact()
    {
        if (consumed) return;
        if (!Unlocked()) return;          // Î´µ½½×¶Î£¬²»¿Éµã

        consumed = true;

        CancelHighlight();

        if (descriptionLines != null && descriptionLines.Length > 0)
            DialogueManager.Instance?.StartDialogue(descriptionLines);
    }
}
