using System.Collections;
using UnityEngine;

public partial class CardEffectProcessor
{
    private void ApplyCorpseExplode(float amount)
    {
        int corpseCount = (int)casterStat.CorpseCount;
        if (corpseCount <= 0) return; //데미지 안줌

        if (targetUI == null) return;
        float ratio = amount * corpseCount;
        float ratioDamage = targetUI.character.currentHp * ratio;
        float totalDamage = ratioDamage
                  + StatWithDirty("casterStat.CorpseDamageAdd");

        totalDamage *= StatWithDirty("casterStat.CorpseDamageMultiple");

        int intDamage = ApplyDamage(totalDamage);

        casterStat.CorpseCount = 0;  // 전부 소모
        AssistDamage(intDamage);
    }
}