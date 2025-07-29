using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class RewardGenerator
{
    //확률
    private Dictionary<int, Dictionary<int, float>> stageTierWeights = new()
    {
        { 0, new() { {1, 1f} } },
        { 1, new() { {1, 0.8f}, {2, 0.2f} } },
        { 2, new() { {1, 0.6f}, {2, 0.3f}, {3, 0.1f} } },
        { 3, new() { {1, 0.4f}, {2, 0.4f}, {3, 0.15f}, {4, 0.05f} } },
        { 4, new() { {1, 0.2f},  {2, 0.3f},  {3, 0.3f},  {4, 0.2f} } },
        { 5, new() { {1, 0.1f},  {2, 0.25f}, {3, 0.35f}, {4, 0.3f} } },
        { 6, new() { {1, 0.0f},  {2, 0.2f},  {3, 0.4f},  {4, 0.4f} } }
    };

    public List<CardData> GetRewardCard()
    {
        int stage = GameManager.gameManager.playerData.currentMap.stageNumber;
        int maxTier = Mathf.Clamp(stage, 1, 4);
        string jobName = GameManager.gameManager.playerData.characterData.name;

        MapTheme theme = GameManager.gameManager.playerData.currentMap.theme;
        if (theme == MapTheme.OCEAN)
        {
            BoostHighTier(stage);
        }

        var SharedDeck = CardGenerator.LoadDeck().cards;
        var jobDeck = CardGenerator.LoadDeck(jobName).cards;

        List<CardData> allCandidates = new();
        allCandidates.AddRange(SharedDeck);
        allCandidates.AddRange(jobDeck);

        var validCards = allCandidates.Where(card => (int)card.rare <= maxTier).ToList();

        if (validCards.Count == 0)
        {
            Debug.LogWarning($"[RewardGenerator] 보상 후보 카드 없음: job={jobName}, stage={stage}");
            return null;
        }

        Dictionary<int, List<CardData>> tierGroups = validCards
            .GroupBy(card => (int)card.rare)
            .ToDictionary(g => g.Key, g => g.ToList());


        if (!stageTierWeights.TryGetValue(stage, out var tierWeights))
        {
            Debug.LogWarning($"[RewardGenerator] stageTierWeights에 스테이지 {stage}가 없음");
            return new List<CardData>();
        }

        List<CardData> rewards = new();
        int tryCount = 0;

        while (rewards.Count < 3 && tryCount++ < 100)
        {
            int tier = PickTierWeighted(tierWeights);
            if (!tierGroups.TryGetValue(tier, out var tierCards) || tierCards.Count == 0)
                continue;

            var candidate = tierCards[Random.Range(0, tierCards.Count)];
            if (!rewards.Contains(candidate))
                rewards.Add(candidate);
        }

        return rewards;
    }

    private int PickTierWeighted(Dictionary<int, float> tierWeights)
    {
        float total = tierWeights.Values.Sum();
        float roll = Random.Range(0f, total);
        float accum = 0f;

        foreach (var kv in tierWeights.OrderBy(kv => kv.Key))
        {
            accum += kv.Value;
            if (roll <= accum)
                return kv.Key;
        }

        return tierWeights.Keys.Max(); // fallback
    }

    private void BoostHighTier(int stage, float boostAmount = 0.05f)
    {
        if (!stageTierWeights.ContainsKey(stage))
        {
            Debug.LogWarning($"[TierWeightTable] 존재하지 않는 스테이지: {stage}");
            return;
        }

        var weights = stageTierWeights[stage];
        int maxTier = weights.Keys.Max();

        // 분배할 낮은 티어들
        var lowerTiers = weights.Keys.Where(t => t != maxTier).ToList();

        if (lowerTiers.Count == 0)
        {
            Debug.LogWarning($"[TierWeightTable] 낮은 티어가 없음 (티어 {maxTier} 하나만 존재)");
            return;
        }

        // 각 낮은 티어에서 깎을 양
        float cutPerTier = boostAmount / lowerTiers.Count;

        // 복사해서 수정
        Dictionary<int, float> newWeights = new(weights);

        foreach (int tier in lowerTiers)
        {
            newWeights[tier] = Mathf.Max(0f, newWeights[tier] - cutPerTier);
        }

        newWeights[maxTier] += boostAmount;

        // 정규화 (합계 == 1.0 보장)
        float total = newWeights.Values.Sum();
        foreach (int key in newWeights.Keys.ToList())
        {
            newWeights[key] /= total;
        }

        // 저장
        stageTierWeights[stage] = newWeights;
    }
}
