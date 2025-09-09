using UnityEngine;
using UnityEngine.Events;

public class ClickPhase : MonoBehaviour
{
    [Header("Ŀ����̨��")]
    public int target = 3;
    [TextArea] public string[] linesWhenReached;

    [Header("������ֵ�󴥷����¼����Ի��������ٴ�����")]
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
                onPhaseCompleted?.Invoke(); // ������������
            });
        }
    }
}
