using System.Collections.Generic;
using UnityEngine;

public class DeckManager : MonoBehaviour
{
    //플레이어 덱을 받고 그걸 관리함
    public List<CardData> drawPile = new(); // 뽑기 덱에 있는 카드
    public List<CardData> discardPile = new();  // 버리는 카드
    public List<CardData> hand = new();   // 현재 손에 있는 카드
    public List<CardData> passiveCards = new();  //패시브 카드
    //TODO 숫자 바꿈
    public int maxHandSize = 5;

    //public CardUIManager cardUIManager;

    public void InitDeck()
    {
        var fullDeck = GameManager.gameManager.playerData.playerDeck.allCards;
        drawPile.Clear();
        discardPile.Clear();
        hand.Clear();
        passiveCards.Clear();  // 전부 클리어(아마 필요없긴 할듯)

        foreach (var card in fullDeck)   //패시브, 덱 구별
        {
            if (card.cardType == CardType.Passive)
            {
                passiveCards.Add(card);
            }
            else if (card.cardType == CardType.Scroll)
            {
                hand.Add(card);
            }
            else
            {
                drawPile.Add(card);
            }
        }
    }

    public void Shuffle(List<CardData> list)  // 현재 덱의 카드를 셔플
    {
        for (int i = 0; i < list.Count; ++i)
        {
            int rand = Random.Range(i, list.Count);
            (list[i], list[rand]) = (list[rand], list[i]);
        }
    }



}
