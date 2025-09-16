using System;
using System.Collections.Generic;
using UnityEngine;

public class EffectCardManager : ICloneable
{
    //배틀씬중에서 턴, 횟수 기반 카드를 전부 넣음 (나중에 효과를 되돌리기 위함)

    private List<TurnCountCard> turnCards = new(); //턴 기반 카드 (3턴 적용)
    private List<TurnCountCard> countCards = new(); // 횟수 기반 카드 (3번 공격등)
    public List<string> dirtyFlag = new();

    private StatMultiplier statMultiplier;
    private Character character; //상위 오브젝트인 캐릭터 클래스

    private CoefficientModifier modifier = new();

    public void SetupCh(Character character)
    {
        this.character = character;
    }

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
            tcc.remainTurn--;
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
        RevertCardEffects(toRemove);

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

        RevertCardEffects(toRemove);

        Debug.Log($"[EffectCardManager] {toRemove.Count}개의 횟수기반 카드 효과 제거됨");
    }

    private void RevertCardEffects(List<TurnCountCard> toRemove)
    {
        if (toRemove == null || toRemove.Count == 0) return;

        List<Character> targets = new() { character };

        foreach (var tcc in toRemove)
        {
            CardData reversedCard = CreateReversedCard(tcc.GetCard());
            modifier.ProcessCard(reversedCard, character, targets);
        }
    }

    //효과를 반대로 한 새로운 CardData 객체를 생성
    private CardData CreateReversedCard(CardData original)
    {
        CardData reversed = new CardData();
        reversed.cardTarget = CardTarget.onePlayer;
        reversed.nameKey = original.nameKey;
        reversed.effectMap = new Dictionary<string, float>();

        foreach (var kvp in original.effectMap)
        {
            if (kvp.Value is float f)
            {
                reversed.effectMap[kvp.Key] = -f;
            }
        }

        return reversed;
    }

    //현재 객체를 복사하는 함수
    public object Clone()
    {
        // 1. JSON으로 기본 복사 (값 타입 필드들)
        string json = JsonUtility.ToJson(this);
        EffectCardManager copy = JsonUtility.FromJson<EffectCardManager>(json);

        // 2. 참조 타입 필드 수동 복사
        copy.turnCards = new List<TurnCountCard>(this.turnCards.Count);
        copy.countCards = new List<TurnCountCard>(this.countCards.Count);
        copy.dirtyFlag = new List<string>(this.dirtyFlag);

        // 3. TurnCountCard 복사 (필요시 깊은 복사)
        foreach (var tcc in this.turnCards)
        {
            copy.turnCards.Add(new TurnCountCard(tcc.GetCard())); // 새 객체 생성
        }
        foreach (var tcc in this.countCards)
        {
            copy.countCards.Add(new TurnCountCard(tcc.GetCard()));
        }

        // 4. StatMultiplier 깊은 복사
        if (this.statMultiplier != null)
        {
            copy.statMultiplier = (StatMultiplier)this.statMultiplier.Clone();
        }

        return copy;
    }

    public void ClearWithoutApply()
    {
        //TODO 현재 적용된 모든 카드의 디버프를 백하지 않고 전부 해제
        turnCards.Clear();
        countCards.Clear();
    }

    public void ClearWithApply()
    {
        //TODO 현재 적용된 모든 카드 버프/디버프를 백하고 해제
        RevertCardEffects(turnCards);
        RevertCardEffects(countCards);
    }

}
