using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class RuneManager : MonoBehaviour
{
    [SerializeField] private Transform runeParent;
    [SerializeField] private GameObject runePrefab;
    [SerializeField] private List<RectTransform> runeSlots;  // 룬이 생성될 위치

    void Start()
    {
        ShowRunes();
    }

    private void ShowRunes()
    {
        RuneData runeData = GameManager.gameManager.runeData;  // 룬 데이터
        var runes = runeData.runes.Where(r => r.mapTheme != MapTheme.NOP).ToList();

        int count = Mathf.Min(runes.Count, runeSlots.Count);

        for (int i = 0; i < count; ++i)
        {
            var rune = runes[i];
            var slot = runeSlots[i];

            GameObject go = Instantiate(runePrefab, runeParent);
            var rt = go.GetComponent<RectTransform>();

            if (rt != null)
            {
                rt.anchoredPosition = slot.anchoredPosition;
            }

            var ui = go.GetComponent<RuneUI>();
            if (ui != null)
            {
                ui.Setup(rune);
            }
        }
    }
}
