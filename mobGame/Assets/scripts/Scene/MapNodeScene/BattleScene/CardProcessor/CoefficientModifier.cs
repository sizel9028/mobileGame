using System.Collections.Generic;
using UnityEngine;

public partial class CoefficientModifier
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
        //특수 케이스 먼저 찾음
        switch (effectKey)
        {
            case "Rage":
                ApplyRage(value, caster, target);
                return;


                // 여기까지는 특수 케이스
        }

        // 나머지는 statMultiplier에서 자동 처리
        var statObj = target.statMultiplier;

        var field = statObj.GetType().GetField(effectKey,
            System.Reflection.BindingFlags.Public |
            System.Reflection.BindingFlags.Instance);

        if (field != null && field.FieldType == typeof(float))
        {
            float current = (float)field.GetValue(statObj);
            field.SetValue(statObj, current + value);
        }
    }

}
