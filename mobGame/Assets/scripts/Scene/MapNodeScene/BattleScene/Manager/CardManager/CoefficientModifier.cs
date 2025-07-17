using System.Collections.Generic;
using UnityEngine;

public class CoefficientModifier 
{

    public void ProcessCardEffect(CardData card, List<Character> targets)
    {
        foreach (var effectKey in card.effectMap.Keys)
        {
            float effectValue = card.effectMap[effectKey];

            if (card.cardTarget == CardTarget.oneEnemy || card.cardTarget == CardTarget.onePlayer)
            {
                if (targets.Count > 0)
                {
                    ApplySingle(effectKey, effectValue, targets[0]);
                }
            }
            else
            {
                ApplyMultiple(effectKey, effectValue, targets);
            }
        }
    }

    private void ApplyMultiple(string effectKey, float value, List<Character> targets)
    {
        foreach (var target in targets)
        {
            ApplySingle(effectKey, value, target);
        }
    }

    private void ApplySingle(string effectKey, float value, Character target)
    {
        //TODO 계수 변경
    }
}
