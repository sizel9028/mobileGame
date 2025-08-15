using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;


// 움직이는 룬 만듬
public class FloatingRuneUI : MonoBehaviour
{
    [SerializeField] private Image runeImage;
    [SerializeField] private LocalizedText nameText;
    [SerializeField] private LocalizedText descText;
    private RectTransform rectTransform;
    private Vector2 startPos;

    [Header("부유 설정")]
    public float floatDistance = 10f;
    public float floatDuration = 1.5f;

    private RuneProcessor runeProcessor = new();

    void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        startPos = rectTransform.anchoredPosition;

        rectTransform.DOAnchorPosY(startPos.y + floatDistance, floatDuration)
            .SetEase(Ease.InOutSine)
            .SetLoops(-1, LoopType.Yoyo);

        nameText.skipStart = true;
        descText.skipStart = true;
        nameText.Clear();
        descText.Clear();
    }

    public void DisplayRune(Rune data)
    {
        nameText.SetText(data.GetNameKey());

        if (data.level != 0)
        {
            descText.SetText(data.GetDescKey());
            float effectVal = runeProcessor.GetRuneCoefficient(data);
            descText.AppendText(GetEffectDisplay(data, effectVal));
        }
        else
        {
            descText.SetText($"rune_{data.mapTheme}_0level");
        }

        nameText.AppendText($" (lv{data.level})");
        LoadRuneArt(data);
    }

    private string GetEffectDisplay(Rune rune, float val)
    {
        return rune.mapTheme switch
        {
            MapTheme.FOREST => $" (+{val * 100:0.#}%)",  // 흡혈 비율
            MapTheme.OCEAN => $" (+{(int)val})",         // 리셋
            MapTheme.VOID => $" (+{(int)val} HP)",      // 최대 체력 증가
            _ => $" (+{val})"
        };
    }

    private void LoadRuneArt(Rune data)
    {
        Sprite art = Resources.Load<Sprite>(data.GetArtPath());

        if (art != null)
        {
            runeImage.sprite = art;
        }
        else
        {
            Debug.LogWarning($"[RuneUI] 이미지 로드 실패: {data.GetArtPath()}");
        }
    }
}
