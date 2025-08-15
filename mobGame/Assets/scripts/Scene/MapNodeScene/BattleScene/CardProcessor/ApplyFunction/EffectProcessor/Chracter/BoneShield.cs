using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class CardEffectProcessor
{
    private void ApplyBoneShield(float amount)
    {
        Debug.Log("본실드 실행");
        var count = casterStat.CorpseCount;

        if (count < 5) return;

        CardData tmpCard = new CardData();
        tmpCard.effectMap = new Dictionary<string, float>
        {
            {"incomingDamage", -amount}
        }; //얼만큼 감소하는지 받는 데미지가
        tmpCard.maxCount = 3; //세번만 지속
        tmpCard.cardTarget = CardTarget.onePlayer;

        modifier.ProcessCardEffect(tmpCard, casterUI, new List<CharacterUI> { casterUI });
    }
}