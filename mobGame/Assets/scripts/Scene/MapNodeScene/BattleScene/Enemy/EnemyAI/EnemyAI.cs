using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public enum EnemyPlayType
{
    Attack, Shield, Heal, BuffSelf, DebuffEnemy, Summon, Nop
}
public partial class EnemyAI
{
    private List<CardData> usableCards = new();
    private bool useHpCard = true;
    public void SetCards(List<CardData> handCards, Transform simRoot)
    {
        this.simRoot = simRoot;
        //적이 사용가능한 카드의 집합을 모아둠
        usableCards = handCards.Where(card => CardValidator.IsCardAble(card, isPlayer: false)).ToList();
    }

    //OTK를 실행
    public void PlayCard()
    {
        useHpCard = true;
        EnemyManaSystem.Instance.refillMana();

        Debug.Log($"[EnemyAI] usableCards.Count = {usableCards.Count}");

        var (combo, isOtk) = OneTurnKill(usableCards);
        EnemyManaSystem.Instance.refillMana();

        if (isOtk)
        {
            //무조건 원턴킬 콤보 사용
            PlayOtk(combo);
            Debug.Log("원턴킬 파악");
        }
        else  // 25% 확률로 최적 콤보 사용 (딜만함)
        {
            float rand = Random.value;
            if (rand < 0f)
            {
                PlayOtk(combo);
            }
            else
            {
                PlayStrategically();
            }
        }

        ClearSimRoot(); //원턴킬에서 시뮬레이션 돌렸던 모든 UI들을 삭제시킴
    }

    private void ClearSimRoot()
    {
        if (simRoot == null) return;

        for (int i = simRoot.childCount - 1; i >= 0; i--)
        {
            Transform child = simRoot.GetChild(i);
            GameObject.Destroy(child.gameObject);
        }
    }

    private void PlayOtk(List<CardData> combo)
    {
        Debug.Log("[EnemyAI] OTK or 최적 콤보 실행");
        if (combo == null)
        {
            return;
        }

        foreach (var card in combo)
        {
            EnemySystem.Instance.PlayCard(card);
        }

    }

    //전략적 카드 사용
    private void PlayStrategically()
    {
        //카드 사용이 없을때까지 반복

        Debug.Log("[EnemyAI] 전략적 행동 시작");

        while (true)
        {
            usableCards = usableCards
                .Where(card => CardValidator.IsCardAble(card, isPlayer: false))
                .ToList();

            if (!useHpCard)
            {
                usableCards = usableCards
                .Where(card => card.costType != CostType.Hp)  //hp 카드 제외시킴
                .ToList();
            }
            if (usableCards.Count == 0)
            {
                Debug.Log("[EnemyAI] 사용할 수 있는 카드가 없습니다. 전략 종료.");
                break;
            }

            var weightsDic = GetWeightsDic();

            if (weightsDic.Count == 0)
            {
                Debug.LogWarning("[EnemyAI] weightsDic.Count == 0. 전략 종료.(발생 불가능한 일)");
                break;
            }

            EnemyPlayType chosenType = ChooseByWeight(weightsDic);

            if (chosenType == EnemyPlayType.Nop)
            {
                useHpCard = false;
                Debug.Log("[EnemyAI] Nop 선택 → 이후 체력 소모 카드 금지");
                continue;
            }

            // 해당 타입 카드 중 랜덤 선택
            var candidates = usableCards
                .Where(card => Classify(card) == chosenType)
                .ToList();

            if (candidates.Count == 0)
            {
                Debug.Log($"[EnemyAI] {chosenType} 타입 카드가 없습니다. 다시 시도.");
                continue;
            }

            var chosenCard = candidates[Random.Range(0, candidates.Count)]; //랜덤 선택

            Debug.Log($"[EnemyAI] 전략적 선택: {chosenType} → {chosenCard.nameKey}");
            EnemySystem.Instance.PlayCard(chosenCard);

            usableCards.Remove(chosenCard); //사용한 카드는 삭제
        }
    }

    //가중치 기반 enemyPlayType을 어떤걸 할지 반환함
    private EnemyPlayType ChooseByWeight(Dictionary<EnemyPlayType, float> weightsDic)
    {
        var filtered = weightsDic.Where(pair => pair.Value > 0f).ToList();

        if (filtered.Count == 0)
        {
            Debug.LogWarning("ChooseByWeight: 모든 가중치가 0이어서 Nop 반환");
            return EnemyPlayType.Nop; // 또는 기본값
        }

        float total = filtered.Sum(pair => pair.Value);
        float rand = Random.value * total;

        foreach (var pair in filtered)
        {
            rand -= pair.Value;
            if (rand <= 0f)
                return pair.Key;
        }

        // 혹시라도 부동소수점 문제로 도달하지 못했을 경우
        return filtered.First().Key;
    }

    private Dictionary<EnemyPlayType, float> GetWeightsDic()
    {
        var weightsDic = InitDic();

        //상황 별로 가중치를 다루게 둠
        AdjustWeights(weightsDic);

        return weightsDic;
    }

    private Dictionary<EnemyPlayType, float> InitDic()
    {
        var dic = new Dictionary<EnemyPlayType, float>();

        foreach (var card in usableCards)
        {
            var type = Classify(card);

            if (type == null) continue;

            if (!dic.ContainsKey(type.Value))
            {
                dic[type.Value] = 1f;  // 초기 가중치
            }
        }

        return dic;
    }

    private EnemyPlayType? Classify(CardData card) {
        var effects = card.effectMap;

        if (effects.ContainsKey("Damage"))
            return EnemyPlayType.Attack;

        if (effects.ContainsKey("Shield"))
            return EnemyPlayType.Shield;

        if (effects.ContainsKey("Heal"))
            return EnemyPlayType.Heal;

        if (effects.ContainsKey("Summon"))
            return EnemyPlayType.Summon;

        if (card.actionType == ActionType.Power)
        {
            if (card.cardTarget == CardTarget.allEnemy || card.cardTarget == CardTarget.oneEnemy)
                return EnemyPlayType.DebuffEnemy;

            if (card.cardTarget == CardTarget.allPlayer || card.cardTarget == CardTarget.onePlayer)
                return EnemyPlayType.BuffSelf;
        }

        if (card.costType == CostType.Hp)
            return EnemyPlayType.Nop;

        return null;
    }
}
