using UnityEngine;
using UnityEngine.UI;

public class RuneUI : MonoBehaviour
{
    public LocalizedText nameText;
    public LocalizedText descText;
    [SerializeField] private Image runeImage;

    private RuneProcessor runeProcessor = new();

    public void Setup(Rune data)
    {
        nameText.SetText(data.GetNameKey());
        if (data.level != 0)
        {
            descText.SetText(data.GetDescKey());
            float effectVal = runeProcessor.GetRuneCoefficient(data);
            descText.AppendText(GetEffectDisplay(data, effectVal));
        }
        else descText.SetText($"theme_{data.mapTheme}_0level");

        nameText.AppendText($" (lv{data.level})");

        LoadRuneArt(data);
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
    
    //비율이면 % 아니면 숫자로 나옴
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
}
