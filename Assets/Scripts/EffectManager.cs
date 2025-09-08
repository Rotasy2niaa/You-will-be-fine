using UnityEngine;
using UnityEngine.Rendering;
using System.Collections;

public class EffectManager : MonoBehaviour
{
    [Header("Volumes")]
    public Volume baseVolume;     // ��ѡ����������(ͨ�� weight=1)
    public Volume effectVolume;   // �����˾�(��ʼ weight=0, Priority����)

    [Header("Canvas Blackout (optional)")]
    public CanvasGroup blackoutUI;     // ȫ����Ļ(�ɿ�)
    public bool blockInputWhileActive = true;  // ����ʱ�� UI ����
    [Range(0f, 1f)] public float blackoutMaxAlpha = 0.35f;

    [Header("Timings")]
    public float fadeInTime = 1.5f;  // ���뵽����
    public float holdTime = 0.0f;  // ����ʱ��(0=������)
    public float fadeOutTime = 1.2f;  // ����������

    [Header("Pulse (breathing/heartbeat)")]
    public bool enablePulse = true;  // �Ƿ����ú�Ļ����
    public float pulseSpeed = 4.5f;  // Ƶ��
    [Range(0f, 1f)] public float pulseDepth = 0.25f; // ��������(ռ blackoutMaxAlpha �ı���)

    [SerializeField] private AudioSource breathingSFX;

    Coroutine running;

    void Awake()
    {
        // ��ʼ��״̬
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

    // һ���������(��ѡ����)������
    public void TriggerAttack()
    {
        RestartRoutine(Sequence());
    }

    // ֻ����(���ַ���״̬���ȴ����ڱ𴦵��� Recover)
    public void FadeInOnly()
    {
        RestartRoutine(Blend(effectVolume, effectVolume ? effectVolume.weight : 0f, 1f, fadeInTime, true));
    }

    // ֻ����(���ָ̻�)
    public void FadeOutOnly()
    {
        RestartRoutine(Blend(effectVolume, effectVolume ? effectVolume.weight : 1f, 0f, fadeOutTime, true));
    }

    // �ⲿֱ�ӻָ�
    public void Recover()
    {
        FadeOutOnly();
    }

    // ���� �ڲ�ʵ�� ���� //
    void RestartRoutine(IEnumerator co)
    {
        if (running != null) StopCoroutine(running);
        running = StartCoroutine(co);
    }

    IEnumerator Sequence()
    {
        while (true)
        {
            // ����
            yield return Blend(effectVolume, 0f, 1f, fadeInTime, true);

            // ����
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

            // ����
            yield return Blend(effectVolume, 1f, 0f, fadeOutTime, true);
        }

        running = null;
    }

    IEnumerator Blend(Volume vol, float from, float to, float duration, bool syncBlackout)
    {
        if (!vol) yield break;

        // ������ʼ����ѡ�� UI
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

        // �����������ſ� UI
        if (blackoutUI && blockInputWhileActive && to <= 0f)
        {
            blackoutUI.blocksRaycasts = false;
            blackoutUI.interactable = false;
        }
    }

    // �ú�Ļ��������΢���������/�����У�
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
        // 0..1 ����������
        float pulse = (Mathf.Sin(Time.time * pulseSpeed) * 0.5f + 0.5f); // [0..1]
        float delta = blackoutMaxAlpha * pulseDepth * (pulse - 0.5f) * 2f; // [-depth..+depth] * maxAlpha
        return Mathf.Clamp01(baseAlpha + delta);
    }
}
