using UnityEngine;

public class DemoKeys : MonoBehaviour
{
    public EffectManager fx;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1)) fx.TriggerAttack(); // �����(����)������
        if (Input.GetKeyDown(KeyCode.Alpha2)) fx.FadeInOnly();    // ֻ���룬��������С��Ϸ���ֶ��ָ�
        if (Input.GetKeyDown(KeyCode.Alpha3)) fx.Recover();       // �ָ�
    }
}
