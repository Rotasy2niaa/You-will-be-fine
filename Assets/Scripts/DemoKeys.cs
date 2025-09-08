using UnityEngine;

public class DemoKeys : MonoBehaviour
{
    public EffectManager fx;

    private void Awake()
    {
        fx.TriggerAttack();
    }

    void Update()
    {
        //if (Input.GetKeyDown(KeyCode.Alpha1)) fx.TriggerAttack(); // 淡入→(保持)→淡出
        //if (Input.GetKeyDown(KeyCode.Alpha2)) fx.FadeInOnly();    // 只淡入，等你做完小游戏后手动恢复
        //if (Input.GetKeyDown(KeyCode.Alpha3)) fx.Recover();       // 恢复
    }
}
