using System.Collections.Generic;
using UnityEngine;

public class CardEffectProcessor
{
    public void ProcessCardEffect(CardData card, Character caster, List<Character> targets)
    {
        foreach (var effectKey in card.effectMap.Keys)
        {
            var effect = card.effectMap[effectKey];

            if (card.cardTarget == CardTarget.oneEnemy || card.cardTarget == CardTarget.onePlayer)
            {
                if (targets.Count > 0)
                {
                    ApplyEffectSingle(effectKey, effect, caster, targets[0]);
                }
            }
            else
            {
                ApplyEffectMultiple(effectKey, effect, caster, targets);
            }
        }

        //카드 한장이 사용되면 계수카드 쓰인거 삭제
        caster.effectCardManager.CheckCount();
        foreach (var target in targets)
        {
            target.effectCardManager.CheckCount();
        }

    }

    //target이 여러명일때
    private void ApplyEffectMultiple(string effectKey, float effect, Character caster, List<Character> targets)
    {
        foreach (var target in targets)
        {
            ApplyEffectSingle(effectKey, effect, caster, target);
        }
    }

    //target이 single일때
    private void ApplyEffectSingle(string effectKey, float effect, Character caster, Character target)
    {
        //TODO switch문으로 효과 나누기
    }
}
