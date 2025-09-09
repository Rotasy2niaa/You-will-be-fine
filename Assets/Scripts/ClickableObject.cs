using UnityEngine;

public class ClickableObject : MonoBehaviour
{
    [Header("阶段控制")]
    [Tooltip("该物体从第几轮开始可点击")]
    public int requiredStage = 1;   // med 设成 3

    [Header("外观")]
    //private Renderer rend;
    //private Color originalColor;
    //public Color hoverColor = new Color(1f, 0.5f, 0.5f);

    [Header("描述对话")]
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
        return DialogueManager.Instance == null
            || DialogueManager.Instance.CurrentStage >= requiredStage;
    }

    //void OnMouseEnter()
    //{
    //    Debug.Log(transform.name);
    //    if (consumed) return;
    //    if (!Unlocked()) return;          // 未到阶段，不高亮
    //    //if (rend) rend.material.color = hoverColor;
    //    transform.Find("outline").gameObject.SetActive(true);
    //}

    //void OnMouseExit()
    //{
    //    if (consumed) return;
    //    if (!Unlocked()) return;          // 未到阶段，不变化
    //    //if (rend) rend.material.color = originalColor;
    //    transform.Find("outline").gameObject.SetActive(false);
    //}

    //void OnMouseDown()
    //{
    //    if (consumed) return;
    //    if (!Unlocked()) return;          // 未到阶段，不可点

    //    consumed = true;
    //    //if (rend) rend.material.color = originalColor;
    //    transform.Find("outline").gameObject.SetActive(false);

    //    if (descriptionLines != null && descriptionLines.Length > 0)
    //        DialogueManager.Instance?.StartDialogue(descriptionLines);

    //    // TODO: 这里照旧做你的计数：clickPhase.RegisterClick();
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
        if (!Unlocked()) return;          // 未到阶段，不可点

        consumed = true;

        CancelHighlight();

        if (descriptionLines != null && descriptionLines.Length > 0)
            DialogueManager.Instance?.StartDialogue(descriptionLines);
    }
}
