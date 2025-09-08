using UnityEngine;
using UnityEngine.Rendering;
using System.Collections;

public class EffectManager : MonoBehaviour
{
    [Header("Volumes")]
    public Volume baseVolume;     // 可选：正常那套(通常 weight=1)
    public Volume effectVolume;   // 发作滤镜(初始 weight=0, Priority更高)

    [Header("Canvas Blackout (optional)")]
    public CanvasGroup blackoutUI;     // 全屏黑幕(可空)
    public bool blockInputWhileActive = true;  // 发作时拦 UI 输入
    [Range(0f, 1f)] public float blackoutMaxAlpha = 0.35f;

    [Header("Timings")]
    public float fadeInTime = 1.5f;  // 淡入到发作
    public float holdTime = 0.0f;  // 保持时长(0=不保持)
    public float fadeOutTime = 1.2f;  // 淡出回正常

    [Header("Pulse (breathing/heartbeat)")]
    public bool enablePulse = true;  // 是否启用黑幕脉动
    public float pulseSpeed = 4.5f;  // 频率
    [Range(0f, 1f)] public float pulseDepth = 0.25f; // 脉动幅度(占 blackoutMaxAlpha 的比例)

    [SerializeField] private AudioSource breathingSFX;

    Coroutine running;

    void Awake()
    {
        // 初始化状态
        if (baseVolume) baseVolume.weight = 1f;
        if (effectVolume) effectVolume.weight = 0f;

        if (blackoutUI)
        {
            blackoutUI.alpha = 0f;
            if (blockInputWhileActive)
            {
                blackoutUI.blocksRaycasts = false;
                blackoutUI.interactable = false;
            }
        }
    }

    // 一键：淡入→(可选保持)→淡出
    public void TriggerAttack()
    {
        RestartRoutine(Sequence());
    }

    // 只淡入(保持发作状态，等待你在别处调用 Recover)
    public void FadeInOnly()
    {
        RestartRoutine(Blend(effectVolume, effectVolume ? effectVolume.weight : 0f, 1f, fadeInTime, true));
    }

    // 只淡出(立刻恢复)
    public void FadeOutOnly()
    {
        RestartRoutine(Blend(effectVolume, effectVolume ? effectVolume.weight : 1f, 0f, fadeOutTime, true));
    }

    // 外部直接恢复
    public void Recover()
    {
        FadeOutOnly();
    }

    // —— 内部实现 —— //
    void RestartRoutine(IEnumerator co)
    {
        if (running != null) StopCoroutine(running);
        running = StartCoroutine(co);
    }

    IEnumerator Sequence()
    {
        while (true)
        {
            // 淡入
            yield return Blend(effectVolume, 0f, 1f, fadeInTime, true);

            // 保持
            if (holdTime > 0f)
            {
                float t = 0f;
                while (t < holdTime)
                {
                    t += Time.deltaTime;
                    if (enablePulse) ApplyPulse(effectVolume ? effectVolume.weight : 1f);
                    yield return null;
                }
            }

            // 淡出
            yield return Blend(effectVolume, 1f, 0f, fadeOutTime, true);
        }

        running = null;
    }

    IEnumerator Blend(Volume vol, float from, float to, float duration, bool syncBlackout)
    {
        if (!vol) yield break;

        // 发作开始：可选拦 UI
        if (blackoutUI && blockInputWhileActive && to > from)
        {
            blackoutUI.blocksRaycasts = true;
            blackoutUI.interactable = false;
        }

        duration = Mathf.Max(0.0001f, duration);
        float t = 0f;

        while (t < duration)
        {
            t += Time.deltaTime;
            float p = Mathf.Clamp01(t / duration);

            float w = Mathf.Lerp(from, to, p);
            vol.weight = w;

            if (syncBlackout && blackoutUI)
            {
                float baseAlpha = w * blackoutMaxAlpha;
                if (enablePulse) baseAlpha = ApplyPulseReturn(baseAlpha);
                blackoutUI.alpha = baseAlpha;
            }
            yield return null;
        }

        vol.weight = to;

        if (blackoutUI && syncBlackout)
        {
            float finalAlpha = to * blackoutMaxAlpha;
            if (enablePulse) finalAlpha = ApplyPulseReturn(finalAlpha);
            blackoutUI.alpha = finalAlpha;
        }

        // 发作结束：放开 UI
        if (blackoutUI && blockInputWhileActive && to <= 0f)
        {
            blackoutUI.blocksRaycasts = false;
            blackoutUI.interactable = false;
        }
    }

    // 让黑幕按节奏轻微起伏（呼吸/心跳感）
    void ApplyPulse(float weight01)
    {
        if (!blackoutUI) return;
        blackoutUI.alpha = ApplyPulseReturn(weight01 * blackoutMaxAlpha);
    }

    float ApplyPulseReturn(float baseAlpha)
    {
        // Time.time * 4 / (2.0f * Mathf.PI) * pulseSpeed
        breathingSFX.pitch = (2.0f * Mathf.PI) / (breathingSFX.clip.length * pulseSpeed);
        if (!enablePulse) return baseAlpha;
        // 0..1 的正弦脉动
        float pulse = (Mathf.Sin(Time.time * pulseSpeed) * 0.5f + 0.5f); // [0..1]
        float delta = blackoutMaxAlpha * pulseDepth * (pulse - 0.5f) * 2f; // [-depth..+depth] * maxAlpha
        return Mathf.Clamp01(baseAlpha + delta);
    }
}
