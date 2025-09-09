using UnityEngine;
using UnityEngine.Events;

public class BubblePhase : MonoBehaviour
{
    [Header("Ŀ����̨��")]
    public int target = 3;
    [TextArea] public string[] linesWhenReached;

    [Header("������ֵ�󴥷����¼����Ի��������ٴ�����")]
    public UnityEvent onPhaseCompleted;

    private int count = 0;
    private bool triggered = false;

    public void CountBubble()
    {
        if (triggered) return;
        count++;
        if (count >= target)
        {
            triggered = true;
            // ���ŶԻ����Ի��������ٴ�����һ�׶��¼�
            DialogueManager.Instance.AdvanceStage();
            DialogueManager.Instance.StartDialogue(linesWhenReached, () => {
                onPhaseCompleted?.Invoke(); // ������㡰���� click �Ķ������á���
            });
        }
    }
}
