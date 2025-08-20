using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class CardEffectProcessor
{
    private void ApplySacrifice()
    {
        var playerUI = CharacterUIManager.Instance.playerUIs[0];

        if (targetUI == playerUI) return;

        targetUI.character.currentHp = 0;

        if (Random.value < casterStat.manaGainChanceWithSummon)
        {
            ManaSystem.Instance.Fill(1);
            Debug.Log("[Sacrifice] 소환물 파괴로 마나 +1");
        }

        // 카드 드로우 확률 체크
        if (Random.value < casterStat.drawChanceWithSummon)
        {
            ApplyDraw(1);
            Debug.Log("[Sacrifice] 소환물 파괴로 카드 1장 드로우");
        }
    }
}