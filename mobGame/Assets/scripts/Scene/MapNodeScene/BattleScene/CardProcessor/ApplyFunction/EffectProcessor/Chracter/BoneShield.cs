using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class CardEffectProcessor
{
    private void ApplyBoneShield(float amount)
    {
        int corpseCount = Mathf.Min((int)casterStat.CorpseCount, 5); // 최대 5개 제한

        if (corpseCount < 3) return;

        Debug.Log("본실드 실행");

        CardData tmpCard = new CardData();
        tmpCard.effectMap = new Dictionary<string, float>
        {
            {"incomingDamage", -amount}
        }; //얼만큼 감소하는지 받는 데미지가
        tmpCard.maxCount = 3; //세번만 지속
        tmpCard.cardTarget = CardTarget.onePlayer;

        casterStat.CorpseCount -= corpseCount;

        modifier.ProcessCardEffect(tmpCard, casterUI, new List<CharacterUI> { casterUI });
    }
}