using UnityEngine;
using UnityEngine.UI;

public class RuneUI : MonoBehaviour
{
    [SerializeField] private Image runeImage;
    public Button button;
    public Rune rune;

    void Awake()
    {
        if (button != null)
        {
            button.onClick.RemoveAllListeners();
            button.onClick.AddListener(OnClick);
        }
    }
    public void Setup(Rune data)
    {
        rune = data;
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

    private void OnClick()
    {
        Debug.Log("[RuneUI] 클릭됨");
        FloatingRuneUI floating = FindAnyObjectByType<FloatingRuneUI>();

        if (floating != null)
        {
            floating.DisplayRune(rune);
        }

    }
}
