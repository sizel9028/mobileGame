using DG.Tweening;
using UnityEngine;


// 움직이는 룬 만듬
public class FloatingRuneUI : MonoBehaviour
{
    private RectTransform rectTransform;
    private Vector2 startPos;

    [Header("부유 설정")]
    public float floatDistance = 10f;
    public float floatDuration = 1.5f;

    void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        startPos = rectTransform.anchoredPosition;

        rectTransform.DOAnchorPosY(startPos.y + floatDistance, floatDuration)
            .SetEase(Ease.InOutSine)
            .SetLoops(-1, LoopType.Yoyo);
    }
}
