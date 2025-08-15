using System.Collections;
using UnityEngine;

public partial class CardEffectProcessor
{
    //소환하는 로직
    private void ApplySummon(float amount)
    {
        string monsterName = SummonDataLoader.GetName(amount);
        bool isPlayer = casterUI.character.isPlayer;

        if (string.IsNullOrEmpty(monsterName)) return;

        var characters = isPlayer ? CharacterUIManager.Instance.playerUIs : CharacterUIManager.Instance.enemyUIs;

        int targetSlot = characters.FindIndex(ui => ui == null);

        CharacterUIManager.Instance.AddCharacterByName(monsterName, isPlayer);
        
        if (targetSlot >= 0 && targetSlot < characters.Count)
        {
            CharacterUI summoned = characters[targetSlot];
            if (summoned != null)
            {
                //소환수의 기믹을 채움

                //시체 폭발 
                float rate = casterStat.DeathBlastChance;
                var explodeGimmick = new Gimmick("Explode", rate, 1);
                summoned.character.gimmicks.Add(explodeGimmick);
            }
        }
    }
}