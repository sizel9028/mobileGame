using System.Collections.Generic;
using UnityEngine;

public partial class GimmickManager
{
    private void PlayExplode(Character character, Gimmick gimmick)
    {
        Debug.Log("시체 폭발 사용");
        float factor = character.statMultiplier.DeathBlastDamageFactor;
        int damage = Mathf.RoundToInt(Mathf.Max(character.maxHp * factor * 0.2f, 1));

        var targets = character.isPlayer ? CharacterUIManager.Instance.enemyUIs : CharacterUIManager.Instance.playerUIs;

        CardData tmpCard = new CardData();
        tmpCard.cardTarget = CardTarget.allEnemy;
        tmpCard.effectMap = new Dictionary<string, float>
        {
            { "Damage", damage }
        };

        CharacterUI casterUI = CharacterUIManager.Instance.GetUI(character);
        if (casterUI == null)
        {
            Debug.LogWarning("[PlayExplode] casterUI를 찾을 수 없습니다.");
            return;
        }

        var validTargets = new List<CharacterUI>();
        foreach (var ui in targets)
        {
            if (ui != null && ui.character.currentHp > 0)
                validTargets.Add(ui);
        }

        processor.ProcessCardEffect(tmpCard, casterUI, validTargets);
        gimmick.gimmicCount = 0; 
    }
}
