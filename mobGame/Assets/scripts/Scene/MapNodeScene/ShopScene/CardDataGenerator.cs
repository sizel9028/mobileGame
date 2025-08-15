using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CardDataGenerator
{

    private static readonly Dictionary<CardType, int> cardTypeCounts = new()
    {
        { CardType.Passive, 2 },
        { CardType.Scroll, 3 },
        { CardType.Action, 4 }
    };

    private readonly Dictionary<Rare, float> tierProbabilities = new()
    {
        { Rare.Tier0, 0.4f },
        { Rare.Tier1, 0.3f },
        { Rare.Tier2, 0.2f },
        { Rare.Tier3, 0.1f }
    };

    public List<CardData> GenerateCard()
    {
        string jobName = GameManager.gameManager.playerData.characterData.name;

        var sharedDeck = CardGenerator.LoadDeck().cards;
        var jobDeck = CardGenerator.LoadDeck(jobName).cards;

        List<CardData> allCandidates = new();
        allCandidates.AddRange(sharedDeck);
        allCandidates.AddRange(jobDeck);

        HashSet<CardData> selected = new();

        foreach (var (cardType, count) in cardTypeCounts)
        {
            for (int i = 0; i < count; ++i)
            {
                Rare chosenTier = PickTierWeighted();

                List<CardData> candidates = allCandidates
                    .Where(c => c.rare == chosenTier && c.cardType == cardType && !selected.Contains(c))
                    .ToList();

                if (candidates.Count == 0)
                {
                    Debug.LogWarning($"[ShopCardGenerator] 후보 없음: type={cardType}, tier={chosenTier}");
                    continue;
                }

                var chosen = candidates[Random.Range(0, candidates.Count)];
                selected.Add(chosen);
            }
        }

        List<CardData> result = selected.ToList();

        // 마지막에 삭제 카드 추가 (총 10장 구성)
        result.Add(MkDeleteCard());

        return result;
    }

    public CardData MkDeleteCard()
    {
        return new CardData
        {
            nameKey = "shop_delete_card", // 로컬라이징 키
            descriptionKey = "shop_delete_desc",
            path = "Shop/DeleteCard",     // Resources 폴더 기준 경로 (없어도 됨 실제로 안씀)
            cardArtName = "DeleteCard",   // 아트 파일 이름 (없으면 기본 처리)

            cardType = CardType.Passive,  // 또는 Scroll, 어차피 UI용
            actionType = ActionType.Skill,
            cardTarget = CardTarget.nop,  // 타겟 없음
            rare = Rare.Tier0,            // 의미 없음, 제일 낮은 값 사용

            cost = 0,
            costType = CostType.Mana,     // 의미 없음

            maxTurn = 0,
            maxCount = 0,

            effectMapRaw = "",            // 효과 없음
        };
    }

    private Rare PickTierWeighted()
    {
        float totalWeight = tierProbabilities.Values.Sum();
        float roll = Random.Range(0f, totalWeight);
        float accum = 0f;

        foreach (var kv in tierProbabilities.OrderBy(k => k.Key))
        {
            accum += kv.Value;
            if (roll <= accum)
                return kv.Key;
        }

        return tierProbabilities.Keys.Max(); // fallback
    }
}
