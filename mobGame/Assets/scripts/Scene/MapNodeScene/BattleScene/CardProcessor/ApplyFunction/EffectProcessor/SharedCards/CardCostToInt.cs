using System.Collections;
using UnityEngine;

public partial class CardEffectProcessor
{
    //현재 카드의 코스트를 변경
    private void ApplyCardCostToInt(float amount)
    {
        var decks = DeckManager.Instance.handView.cards;

        foreach (var card in decks)
        {
            card.data.cost = (int)amount;
            card.Setup();
        }
    }
}