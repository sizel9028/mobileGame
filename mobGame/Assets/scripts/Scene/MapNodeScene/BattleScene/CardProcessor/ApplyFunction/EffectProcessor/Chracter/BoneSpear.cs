using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class CardEffectProcessor
{
    private void ApplyBoneSpear(float amount)
    {
        int corpseCount = Mathf.Min((int)casterStat.CorpseCount, 3); // 최대 3개 제한

        if (targetUI == null) return; 
        int maxHp = targetUI.character.maxHp;

        // 피해량 계산
        int damage = 1 + (int)(corpseCount * amount * maxHp);

        // 뼈 소모
        casterStat.CorpseCount -= corpseCount;

        // 데미지 적용
        ApplyDamage(damage);
    }
}