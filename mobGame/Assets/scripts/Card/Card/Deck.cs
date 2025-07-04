using System.Collections.Generic;
using UnityEngine;


public class Deck
{
    public List<CardData> cards = new();
    //public DeckType deckType;

    public void Shuffle()
    {
        for (int i = 0; i < cards.Count; ++i)
        {
            int j = Random.Range(i, cards.Count);
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

}
