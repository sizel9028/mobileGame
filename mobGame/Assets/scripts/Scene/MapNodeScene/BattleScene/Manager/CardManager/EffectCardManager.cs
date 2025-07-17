using System.Collections.Generic;
using UnityEngine;

public class EffectCardManager 
{
    //배틀씬중에서 턴, 횟수 기반 카드를 전부 넣음 (나중에 효과를 되돌리기 위함)

    private List<TurnCountCard> turnCards = new(); //턴 기반 카드 (3턴 적용)
    private List<TurnCountCard> countCards = new(); // 횟수 기반 카드 (3번 공격등)
    public List<string> dirtyFlag = new();

    private StatMultiplier statMultiplier;

    public void Setup(StatMultiplier stat)
    {
        statMultiplier = stat;
    }
    
    public void AddCard(CardData card)
    {
        //TODO 카드 효과 적용 매니저한테 카드 값을 넘겨서 계수 스크립트를 바꿈

        if (card.maxTurn > 0)
        {
            var tcc = new TurnCountCard(card);
            turnCards.Add(tcc);
        }

        if (card.maxCount > 0)
        {
            var tcc = new TurnCountCard(card);
            countCards.Add(tcc);
        }
    }

    public void CheckTurn()
    {
        foreach (var tcc in turnCards)
        {
            tcc.remainCount--;
            Debug.Log($"[EffectCardManager] {tcc.GetCard().nameKey} → 남은 턴: {tcc.remainTurn}");
        }

        TurnClear();
    }

    private void TurnClear()
    {
        List<TurnCountCard> toRemove = turnCards.FindAll(tcc => tcc.remainTurn <= 0);

        foreach (var tcc in toRemove)
        {
            turnCards.Remove(tcc);
        }

        //TODO 턴기반 카드 효과 원래대로 되돌리기

        Debug.Log($"[EffectCardManager] {toRemove.Count}개의 턴기반 카드 효과 제거됨");
    }

    public void CheckCount()
    {
        foreach (var tcc in countCards)
        {
            var card = tcc.GetCard();
            foreach (var effectKey in card.effectMap.Keys)
            {
                if (dirtyFlag.Contains(effectKey))
                {
                    tcc.remainCount--;
                    Debug.Log($"[EffectCardManager] {card.nameKey} → {effectKey} 사용됨 → count 감소: {tcc.remainCount}");
                    break;
                }
            }
        }

        CountClear();
        dirtyFlag.Clear();
    }

    // 이제 카운트 횟수가 남아있지 않는 카드들은 삭제시킨다
    private void CountClear()
    {
        List<TurnCountCard> toRemove = countCards.FindAll(tcc => tcc.remainCount <= 0);

        foreach (var tcc in toRemove)
        {
            countCards.Remove(tcc);
        }

        //TODO toRemove에 담긴 효과 카드 리스트를 넘겨서 계수를 이전으로 복구시키는 작업을 함
    }

}
