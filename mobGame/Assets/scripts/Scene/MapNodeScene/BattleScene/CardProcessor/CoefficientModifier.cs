using System.Collections.Generic;
using UnityEngine;

public class CoefficientModifier 
{

    public void ProcessCardEffect(CardData card, CharacterUI casterUI, List<CharacterUI> targetUIs)
    {
        foreach (var effectKey in card.effectMap.Keys)
        {
            float effectValue = card.effectMap[effectKey];

            if (card.cardTarget == CardTarget.oneEnemy || card.cardTarget == CardTarget.onePlayer)
            {
                if (targetUIs.Count > 0)
                {
                    ApplySingle(effectKey, effectValue, casterUI, targetUIs[0]);
                }
            }
            else
            {
                ApplyMultiple(effectKey, effectValue, casterUI, targetUIs);
            }
        }
    }

    private void ApplyMultiple(string effectKey, float value, CharacterUI casterUI, List<CharacterUI> targetUIs)
    {
        foreach (var targetUI in targetUIs)
        {
            ApplySingle(effectKey, value, casterUI, targetUI);
        }
    }

    private void ApplySingle(string effectKey, float value,CharacterUI casterUI, CharacterUI targetUI)
    {
        //TODO 계수 변경
    }
}
