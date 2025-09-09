using UnityEngine;
using TMPro;
using System;

public class DialogueManager : MonoBehaviour
{
    public static DialogueManager Instance { get; private set; }

    // ====== 新增：全局阶段（从 1 开始）======
    public int CurrentStage { get; private set; } = 1;
    public void SetStage(int stage) => CurrentStage = Mathf.Max(1, stage);
    public void AdvanceStage() => CurrentStage++;

    // 下面保持你原来的对话逻辑即可（若还没写，可忽略）
    [Header("UI")]
    [SerializeField] private GameObject panel;
    [SerializeField] private TextMeshProUGUI textUI;
    private string[] lines;
    private int idx;
    private bool active;
    private Action onComplete;

    void Awake()
    {
        if (Instance && Instance != this) { Destroy(gameObject); return; }
        Instance = this;
        if (panel) panel.SetActive(false);
    }

    void Update()
    {
        if (!active) return;
        if (Input.GetKeyDown(KeyCode.Space)) ShowNext();
    }

    public void StartDialogue(string[] dialogueLines, Action onComplete = null)
    {
        if (dialogueLines == null || dialogueLines.Length == 0) return;
        lines = dialogueLines; idx = 0; this.onComplete = onComplete;
        if (panel) panel.SetActive(true);
        if (textUI) textUI.text = lines[idx];
        active = true;
    }

    void ShowNext()
    {
        idx++;
        if (idx < (lines?.Length ?? 0)) { if (textUI) textUI.text = lines[idx]; }
        else EndDialogue();
    }

    void EndDialogue()
    {
        active = false;
        if (panel) panel.SetActive(false);
        var cb = onComplete; onComplete = null;
        cb?.Invoke();
    }
}
