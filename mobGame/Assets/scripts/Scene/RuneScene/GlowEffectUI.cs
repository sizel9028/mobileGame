using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class GlowEffectUI : MonoBehaviour
{
    private Image glowImage;

    [Header("Glow 설정")]
    public float minAlpha = 0.3f;
    public float maxAlpha = 0.7f;
    public float duration = 1.5f;

    void Start()
    {
        glowImage = GetComponent<Image>();

        Color c = glowImage.color;
        c.a = minAlpha;
        glowImage.color = c;

        glowImage.DOFade(maxAlpha, duration)
            .SetLoops(-1, LoopType.Yoyo)
            .SetEase(Ease.InOutSine);
    }
}
