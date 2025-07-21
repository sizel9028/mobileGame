using System.Collections.Generic;
using UnityEngine;

public class CardEffectProcessor
{
    // 적용할거 매번 적기 귀찮아서 지역변수로 설정
    private StatMultiplier casterStat;
    private StatMultiplier targetStat;
    private CharacterUI casterUI;
    private CharacterUI targetUI;


    public void ProcessCardEffect(CardData card, CharacterUI casterUI, List<CharacterUI> targetUIs)
    {

        foreach (var effectKey in card.effectMap.Keys)
        {
            var effect = card.effectMap[effectKey];

            if (card.cardTarget == CardTarget.oneEnemy || card.cardTarget == CardTarget.onePlayer)
            {
                if (targetUIs.Count > 0)
                {
                    ApplyEffectSingle(effectKey, effect, casterUI, targetUIs[0]);
                }
            }
            else
            {
                ApplyEffectMultiple(effectKey, effect, casterUI, targetUIs);
            }
        }

        //카드 한장이 사용되면 계수카드 쓰인거 삭제
        casterUI.character.effectCardManager.CheckCount();
        foreach (var targetUI in targetUIs)
        {
            targetUI.character.effectCardManager.CheckCount();
        }

    }

    //target이 여러명일때
    private void ApplyEffectMultiple(string effectKey, float effect, CharacterUI casterUI, List<CharacterUI> targetUIs)
    {
        foreach (var targetUI in targetUIs)
        {
            ApplyEffectSingle(effectKey, effect, casterUI, targetUI);
        }
    }

    //target이 single일때
    private void ApplyEffectSingle(string effectKey, float effect, CharacterUI casterUI, CharacterUI targetUI)
    {
        this.casterUI = casterUI;
        this.targetUI = targetUI;
        casterStat = casterUI.character.statMultiplier;
        targetStat = targetUI.character.statMultiplier;

        //TODO switch문으로 효과 나누기
        switch (effectKey)
        {
            case "Damage":
                ApplyDamage(effect);
                break;

        }
    }

    private void ApplyDamage(float damage)
    {
        //실제 입는 데미지
        damage += casterStat.outgoingDamageAdd;
        damage *= casterStat.outgoingDamageMultiple;
        damage *= targetStat.incomingDamage;

        AssistDamage(damage);
    }

    private void AssistDamage(float damage)
    {
        int intDamage = Mathf.RoundToInt(damage);

        if (targetUI != null)
        {
            targetUI.character.TakeDamage(intDamage);
            targetUI.Setup();

            if (intDamage > 0)
            {
                targetUI.Damage();  // 데미지 모션
            }
        }

    }
    
    
}
