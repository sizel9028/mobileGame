using System.Collections;
using UnityEngine;

public partial class CardEffectProcessor
{
    private void ApplySummonBoneGolem(float amount)
    {
        bool isPlayer = casterUI.character.isPlayer;

        // 소환 주체(UI) 찾기 (플레이어 or 적)
        var casterList = isPlayer 
            ? CharacterUIManager.Instance.playerUIs 
            : CharacterUIManager.Instance.enemyUIs;

        if (casterList.Count == 0 || casterList[0] == null) return;

        var leaderStat = casterList[0].character.statMultiplier;
        int corpseCount = (int)leaderStat.CorpseCount;
        if (corpseCount <= 0) return; // 시체 없으면 소환 불가
        casterStat.CorpseCount = 0;   // 전부 소모
        
        var characters = isPlayer ? CharacterUIManager.Instance.playerUIs : CharacterUIManager.Instance.enemyUIs;
        int targetSlot = characters.FindIndex(ui => ui == null);

        // 골렘 소환
        CharacterUIManager.Instance.AddCharacterByName("BoneGolem", isPlayer);

        // 스탯 적용
        if (targetSlot >= 0 && targetSlot < characters.Count)
        {
            CharacterUI summoned = characters[targetSlot];
            if (summoned != null)
            {
                int hp = 20 + (int)(corpseCount * amount);

                summoned.character.maxHp = hp;
                summoned.character.currentHp = hp;

                // 기믹 예시: 사망 시 시체 반환
                var returnCorpseGimmick = new Gimmick("ReturnCorpse", 1f, 2); // 시체 2개 반환
                summoned.character.gimmicks.Add(returnCorpseGimmick);
            }
        }
    }
}