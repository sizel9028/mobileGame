using System.Collections.Generic;
using UnityEngine;

public class CardProcessor
{
    //카드 계수 바꾸는거 인스턴스화
    private CardEffectProcessor effectProcessor = new();
    private CoefficientModifier coefficientModifier = new();


    //카드 데이터와 caster, targets을 넣으면 카드를 적용시킴
    public void ProcessCard(CardData card, Character caster, List<Character> targets)
    {
        switch (card.actionType)
        {
            case ActionType.Skill:
                effectProcessor.ProcessCardEffect(card, caster, targets);
                break;

            case ActionType.Power:
                coefficientModifier.ProcessCardEffect(card, targets);
                break;
        }
    }
}
