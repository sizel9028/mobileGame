using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public partial class CardEffectProcessor
{
    private void ApplyDrawHighTierCard()
    {
        //높은 티어의 카드를 하나 뽑아옴
        var allCards = new List<CardData>();
        allCards.AddRange(DeckManager.Instance.drawPile.cards);
        allCards.AddRange(DeckManager.Instance.discardPile.cards);

        if (allCards.Count == 0)
        {
            //버리는 존과 덱에 카드가 존재하지 않음
            return;
        }

        int maxTier = -1;
        //최대 티어를 고름
        foreach (var card in allCards)
        {
            if ((int)card.rare > maxTier)
            {
                maxTier = (int)card.rare;
            }
        }

        var highTierCards = allCards.Where(card => (int)card.rare == maxTier).ToList();

        int idx = Random.Range(0, highTierCards.Count);
        var selectedCard = highTierCards[idx];

        //고른 카드를 덱 매니저에서 제거시킴
        if (DeckManager.Instance.drawPile.cards.Contains(selectedCard))
            DeckManager.Instance.drawPile.cards.Remove(selectedCard);
        else
            DeckManager.Instance.discardPile.cards.Remove(selectedCard);

        DeckManager.Instance.AddCardUI(selectedCard);
    }
}