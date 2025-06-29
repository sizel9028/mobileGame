using UnityEngine;

public enum CardType
{
    Attack
}

public enum CardTarget
{
    Enemy
}

public class CardData
{
    //카드 설명 이름 localized Text에 해당하는 key
    public string nameKey;
    public string descriptionKey;

    public Sprite cardArt;
    public CardType cardType;
    public CardTarget cardTarget;
    public int cost;
    //TODO 상태이상, 데미지 등등 효과
}
