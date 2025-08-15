using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

//카드 가격 생성
public class CardCostGenerator
{
    //티어별 현재골드% 가격
    private readonly Dictionary<Rare, float> tierPercentage = new()
    {
        { Rare.Tier0, 0.10f },
        { Rare.Tier1, 0.15f },
        { Rare.Tier2, 0.22f },
        { Rare.Tier3, 0.30f }
    };

    private readonly Dictionary<CardType, int> addGoldWithCardType = new()
    {
        { CardType.Passive, 20 },
        { CardType.Scroll, 10 },
        { CardType.Action, 0 }
    };

    private int currentGold;

    void Start()
    {
        currentGold = GameManager.gameManager.playerData.gold;
    }

    public int GetCost(CardData card)
    {
        float percent = tierPercentage.TryGetValue(card.rare, out var p) ? p : 0.2f;
        int baseCost = Mathf.RoundToInt(currentGold * percent);

        int typeFee = addGoldWithCardType.TryGetValue(card.cardType, out var fee) ? fee : 0;

        int seed = card.nameKey.GetHashCode();
        System.Random rng = new(seed);
        int randomAdd = rng.Next(5, 16);  // 5 ~ 15 추후 변경

        int finalCost = baseCost + typeFee + randomAdd;
        return finalCost;
    }
}
