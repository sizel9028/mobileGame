using UnityEngine;

public class TurnCountCard
{
    public int remainTurn;
    public int remainCount;

    private CardData card;

    public TurnCountCard(CardData card)
    {
        SetData(card);
    }

    public void SetData(CardData card)
    {
        this.card = card;
        remainTurn = card.maxTurn;
        remainCount = card.maxCount;
    }

    public CardData GetCard()
    {
        return card;
    }
}
