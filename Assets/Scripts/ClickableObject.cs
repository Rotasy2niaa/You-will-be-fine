using UnityEngine;

public class ClickableObject : MonoBehaviour
{
    [Header("�׶ο���")]
    [Tooltip("������ӵڼ��ֿ�ʼ�ɵ��")]
    public int requiredStage = 1;   // med ��� 3

    [Header("���")]
    //private Renderer rend;
    //private Color originalColor;
    //public Color hoverColor = new Color(1f, 0.5f, 0.5f);

    [Header("�����Ի�")]
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
    //    if (!Unlocked()) return;          // δ���׶Σ�������
    //    //if (rend) rend.material.color = hoverColor;
    //    transform.Find("outline").gameObject.SetActive(true);
    //}

    //void OnMouseExit()
    //{
    //    if (consumed) return;
    //    if (!Unlocked()) return;          // δ���׶Σ����仯
    //    //if (rend) rend.material.color = originalColor;
    //    transform.Find("outline").gameObject.SetActive(false);
    //}

    //void OnMouseDown()
    //{
    //    if (consumed) return;
    //    if (!Unlocked()) return;          // δ���׶Σ����ɵ�

    //    consumed = true;
    //    //if (rend) rend.material.color = originalColor;
    //    transform.Find("outline").gameObject.SetActive(false);

    //    if (descriptionLines != null && descriptionLines.Length > 0)
    //        DialogueManager.Instance?.StartDialogue(descriptionLines);

    //    // TODO: �����վ�����ļ�����clickPhase.RegisterClick();
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
        if (!Unlocked()) return;          // δ���׶Σ����ɵ�

        consumed = true;

        CancelHighlight();

        if (descriptionLines != null && descriptionLines.Length > 0)
            DialogueManager.Instance?.StartDialogue(descriptionLines);
    }
}
