using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Deck
{
    public List<CardData> cards = new();
    //public DeckType deckType;

    public void Shuffle()
    {
        for (int i = 0; i < cards.Count; ++i)
        {
            int j = UnityEngine.Random.Range(i, cards.Count);
            (cards[i], cards[j]) = (cards[j], cards[i]);
        }
    }

    public CardData Draw()
    {
        if (cards.Count == 0) return null;

        var card = cards[0];
        cards.RemoveAt(0);
        return card;
    }

    public CardData PeekDraw(int index)
    {
        if (index < 0 || index >= cards.Count) return null;

        return cards[index];
    }

}
