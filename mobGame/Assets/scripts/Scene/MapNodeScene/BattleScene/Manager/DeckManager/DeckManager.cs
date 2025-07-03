using System.Collections.Generic;
using UnityEngine;

public class DeckManager : MonoBehaviour
{
    //플레이어 덱을 받고 그걸 관리함
    public Deck drawPile = new(); // 뽑기 덱에 있는 카드
    public Deck discardPile = new();  // 버리는 카드
    //public List<CardData> hand = new();   // 현재 손에 있는 카드
    public List<CardData> passiveCards = new();  //패시브 카드
    //TODO 숫자 바꿈
    public int maxHandSize = 8;
    public int firstDraw = 3;

    public Transform handPanel;
    public CardUIManager cardUIManager;  // hand 대신

    public void InitDeck()
    {
        var fullDeck = GameManager.gameManager.playerData.playerDeck;
        drawPile.cards.Clear();
        discardPile.cards.Clear();
        passiveCards.Clear();  // 전부 클리어
        cardUIManager.handCards.Clear();

        foreach (var card in fullDeck.cards)   //패시브, 덱 구별
        {
            if (card.cardType == CardType.Passive)
            {
                passiveCards.Add(card);
            }
            else if (card.cardType == CardType.Scroll)
            {
                cardUIManager.Register(card, handPanel);
            }
            else
            {
                drawPile.cards.Add(card);
            }
        }

        drawPile.Shuffle();
        for (int i = 0; i < firstDraw; ++i)
        {
            DrawCard();
        }
        cardUIManager.ArrangeCardWithArc();
    }

    public void DrawCard()
    {
        if (cardUIManager.handCards.Count >= maxHandSize)
        {
            Debug.Log("Hand is full");
            return;
        }
        //덱에 카드가 없으면 discard부터 접근, 있으면 그걸 덱에 넣음
        if (drawPile.cards.Count == 0)
        {
            drawPile.cards.AddRange(discardPile.cards);
            discardPile.cards.Clear();
            drawPile.Shuffle();
        }

        var card = drawPile.Draw();
        if (card != null)
        {
            cardUIManager.Register(card, handPanel);
        }
    }

    public void DiscardCard(CardData card)
    {

    }
}
