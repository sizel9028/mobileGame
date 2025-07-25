using System.Collections.Generic;
using UnityEngine;

public class CoefficientModifier
{
    public void ProcessCardEffect(CardData card, CharacterUI playerUI, List<CharacterUI> enemyUIs)
    {
        Character playerCh = playerUI.character;
        List<Character> enemyChs = new List<Character>();

        foreach (var enemyUI in enemyUIs)
        {
            if (enemyUI == null) continue;
            enemyChs.Add(enemyUI.character);
        }

        ProcessCard(card, playerCh, enemyChs);
    }

    public void ProcessCard(CardData card, Character caster, List<Character> targets)
    {
        foreach (var effectKey in card.effectMap.Keys)
        {
            float effectValue = card.effectMap[effectKey];

            if (card.cardTarget == CardTarget.oneEnemy || card.cardTarget == CardTarget.onePlayer)
            {
                if (targets.Count > 0)
                {
                    ApplySingle(effectKey, effectValue, caster, targets[0]);
                }
            }
            else
            {
                ApplyMultiple(effectKey, effectValue, caster, targets);
            }
        }

        //계수를 변경한거니깐 targets에 적용
        //무한 지속카드 (0,0)은 추가하지 않음
        if (card.maxTurn > 0 || card.maxCount > 0)
        {
            foreach (var target in targets)
            {
                target.effectCardManager.AddCard(card);
            }
        }
    }

    private void ApplyMultiple(string effectKey, float value, Character caster, List<Character> targets)
    {
        foreach (var target in targets)
        {
            ApplySingle(effectKey, value, caster, target);
        }
    }

    private void ApplySingle(string effectKey, float value, Character caster, Character target)
    {
        //TODO 계수 변경
        switch (effectKey)
        {
            case "incomingDamage":
                target.statMultiplier.incomingDamage += value;
                break;

            case "outgoingDamageAdd":
                target.statMultiplier.outgoingDamageAdd += value;
                break;

            case "outgoingDamageMultiple":
                target.statMultiplier.outgoingDamageMultiple += value;
                break;
        }
    }
    
}
