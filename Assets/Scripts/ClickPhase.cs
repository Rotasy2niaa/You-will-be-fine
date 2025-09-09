using UnityEngine;
using UnityEngine.Events;

public class ClickPhase : MonoBehaviour
{
    [Header("目标与台词")]
    public int target = 3;
    [TextArea] public string[] linesWhenReached;

    [Header("到达阈值后触发的事件（对话结束后再触发）")]
    public UnityEvent onPhaseCompleted;

    private int count = 0;
    private bool triggered = false;

    public void RegisterClick()
    {
        if (triggered) return;
        count++;
        if (count >= target)
        {
            triggered = true;
            DialogueManager.Instance.StartDialogue(linesWhenReached, () => {
                onPhaseCompleted?.Invoke(); // 继续后续流程
            });
        }
    }
}
