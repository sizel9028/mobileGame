using System.Collections.Generic;
using UnityEngine;

//현재 작용중인 패시브카드를 입력받아서 계수를 작성함
public class PassiveProcessor : Singleton<PassiveProcessor>
{
    public Character playerCh = new();  // 아군 소환시 적용되는 캐릭터
    public Character enemyCh = new();   // 적군 소환시 적용되는 캐릭터
    public CoefficientModifier modifier = new();
    private RuneProcessor runeProcessor = new(); //룬을 수정함

    public void ApplyPassiveCard(List<CardData> playerPassives, List<CardData> enemyPassives)
    {
        runeProcessor.ProcessRuneEffect(playerCh);
        ApplyPassiveToCh(playerCh, playerPassives);
        ApplyPassiveToCh(enemyCh, enemyPassives);
    }

    //Character을 패시브가 적용된 상태로 만든다
    private void ApplyPassiveToCh(Character caster, List<CardData> passives)
    {

        if (passives == null || passives.Count == 0) return;

        foreach (var passiveCard in passives)
        {
            List<Character> targets = GetTargets(passiveCard, caster);
            modifier.ProcessCard(passiveCard, caster, targets);
        }
    }

    //cardTarget에 따라서 상대적인 Character target을 반환
    private List<Character> GetTargets(CardData card, Character caster)
    {
        switch (card.cardTarget)
        {
            case CardTarget.allPlayer:
                return new List<Character> { caster };

            case CardTarget.allEnemy:
                return new List<Character> { caster == playerCh ? enemyCh : playerCh };

            default:
                Debug.LogWarning($"[PassiveProcessor] 알 수 없는 CardTarget: {card.cardTarget}");
                return new List<Character>();
        }
    }
    
}
