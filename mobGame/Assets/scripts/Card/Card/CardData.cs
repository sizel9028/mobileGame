using System;
using UnityEngine;

public enum CardType  //카드 종류 (패시브, 스크롤, 이외 나머지)
{
    Passive, Scroll, Action
}

public enum ActionType  //카드의 세부 타입
{
    Attack, Skill, Power
}

public enum CardTarget
{
    oneEnemy, allEnemy, onePlayer, allPlayer
}

public enum StatusEffect
{
    Stun, Poison, Lifesteal, Weak, Vulnerable,//취약,  // 플레이어가 쓸거, 적도 가능
    Burn
}

public enum Rare
{
    Tier0, Tier1, Tier2, Tier3
}

public class CardData
{
    //카드 설명 이름 localized Text에 해당하는 key
    public string nameKey;
    public string descriptionKey;
    // public string ActiontypeKey; actionType을 보여주는 키

    //카드 타입

    //public Sprite cardArt; 카드 저장 문제 대안으로 이름 저장
    public string path;
    public string cardArtName;
    [NonSerialized] public Sprite cardArt;

    public CardType cardType;
    public ActionType actionType;
    public CardTarget cardTarget;
    public Rare rare;
    public int cost;
    public int damage;  // 카드가 Player/Enemy에 작용하는 숫자

    //TODO 상태이상, 데미지 등등 효과
    public StatusEffect statusEffect;

    public void LoadArt()
    {
        string fullPath = $"{path}/{cardArtName}";
        cardArt = Resources.Load<Sprite>(fullPath);
        if (cardArt == null)
        {
            Debug.LogWarning($"[CardData] 스프라이트 로드 실패: {fullPath}");
        }
    }

}
