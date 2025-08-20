using System;
using System.Collections.Generic;
using UnityEngine;

public enum CardType  //카드 종류 (패시브, 스크롤, 이외 나머지)
{
    Passive, Scroll, Action
}

public enum ActionType  //카드의 세부 타입 (Skill : 계수영향 x, power : 계수를 바꿈)
{
    Skill, Power
}

public enum CardTarget
{
    oneEnemy, allEnemy, onePlayer, allPlayer, nop
}

public enum CostType
{
    Hp, Mana
}

public enum Rare
{
    Tier0, Tier1, Tier2, Tier3, TierRage
}

public class CardData : ICloneable
{
    //카드 설명 이름 localized Text에 해당하는 key
    public string nameKey;
    public string descriptionKey;
    // public string ActiontypeKey; actionType을 보여주는 키

    //카드 타입

    //public Sprite cardArt; 카드 저장 문제 대안으로 이름 저장
    public string path;
    public string cardArtName;
    //[NonSerialized] public Sprite cardArt;

    public CardType cardType;
    public ActionType actionType;
    public CardTarget cardTarget;
    public Rare rare;
    public int cost;
    //public int damage;  // 카드가 Player/Enemy에 작용하는 숫자 effect에서 처리

    //TODO 상태이상, 데미지 등등 효과
    public CostType costType;

    public int maxTurn;
    public int maxCount;

    public string effectMapRaw; // "Damage::20,Heal::20" 같은 효과들 전부 모아둠
    public Dictionary<string, float> effectMap = new();

    public List<Gimmick> gimmickEffects = new();


    public void ParseEffectMap()
    {
        if (effectMap.Count > 0) return;

        effectMap.Clear();
        gimmickEffects.Clear();

        if (string.IsNullOrWhiteSpace(effectMapRaw)) return;

        var entries = effectMapRaw.Split('|');

        foreach (var entry in entries)
        {
            var parts = entry.Split(new[] { "::" }, StringSplitOptions.RemoveEmptyEntries);

            if (parts.Length == 2)
            {
                string key = parts[0].Trim();
                string valueStr = parts[1].Trim();

                // 범위 처리 ex) "1:3"
                if (valueStr.Contains(":"))
                {
                    var range = valueStr.Split(':');
                    if (range.Length >= 2 &&
                        float.TryParse(range[0], out float min) &&
                        float.TryParse(range[1], out float max))
                    {
                        if (min > max)
                        {
                            float tmp = min;
                            min = max;
                            max = tmp;
                        }
                        bool isInt = range.Length == 3 && range[2].ToLower() == "d";

                        if (isInt)
                        {
                            // 정수 랜덤 (max 포함되도록 +1)
                            int randValue = UnityEngine.Random.Range(Mathf.RoundToInt(min), Mathf.RoundToInt(max + 1));
                            effectMap[key] = randValue;
                        }
                        else
                        {
                            // 소수 랜덤 (소수점 둘째 자리까지)
                            float randValue = UnityEngine.Random.Range(min, max);
                            randValue = (float)Math.Round(randValue, 2, MidpointRounding.AwayFromZero);
                            effectMap[key] = randValue;
                        }
                    }
                    else
                    {
                        Debug.LogWarning($"[CardData] 잘못된 범위 값: {valueStr}");
                    }
                }
                else if (float.TryParse(valueStr, out float value))
                {
                    effectMap[key] = value;
                }
                else
                {
                    Debug.LogWarning($"[CardData] 일반 효과 파싱 실패: {entry}");
                }
            }
            else if (parts.Length == 4 && parts[0] == "Gimmick")
            {
                // 기믹 효과
                string gimmickName = parts[1];
                float condition = float.Parse(parts[2]);
                int gimmickValue = int.Parse(parts[3]);
                gimmickEffects.Add(new Gimmick(gimmickName, condition, gimmickValue));
            }
            else
            {
                Debug.LogWarning($"[CardData] 효과 파싱 실패: {entry}");
            }
        }
    }

    public object Clone()
    {
        CardData copy = new CardData
        {
            nameKey = this.nameKey,
            descriptionKey = this.descriptionKey,
            path = this.path,
            cardArtName = this.cardArtName,
            cardType = this.cardType,
            actionType = this.actionType,
            cardTarget = this.cardTarget,
            rare = this.rare,
            cost = this.cost,
            costType = this.costType,
            maxTurn = this.maxTurn,
            maxCount = this.maxCount,
            effectMapRaw = this.effectMapRaw
        };

        foreach (var kv in this.effectMap)
        {
            copy.effectMap[kv.Key] = kv.Value;
        }

        foreach (var g in this.gimmickEffects)
        {
            copy.gimmickEffects.Add(new Gimmick(g.gimmickName, g.gimmicCondition, g.gimmicCount));
        }

        return copy;
    }

}
