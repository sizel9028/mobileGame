using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeckManager : Singleton<DeckManager>
{
    //플레이어 덱을 받고 그걸 관리함
    public Deck drawPile = new(); // 뽑기 덱에 있는 카드
    public Deck discardPile = new();  // 버리는 카드
    //public List<CardData> hand = new();   // 현재 손에 있는 카드
    public List<CardData> passiveCards = new();  //패시브 카드
    public List<CardData> scrollCards = new();
    //TODO 숫자 바꿈
    public int maxHandSize = 9;
    private int firstDraw = 4;

    public Transform handPanel;
    public CardUIManager cardUIManager;    // 이외의 모든 UI관리
    public HandView handView;   // 손에 보이게 함



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
                scrollCards.Add(card);
                //cardUIManager.Register(card, handPanel);
                //무조건 스크롤타입이라고 바로 넣지말고 나중에 최대 4장만 넣게하자
            }
            else
            {
                drawPile.cards.Add(card);
            }
        }

        StartCoroutine(DrawOpeningHand());

    }

    public void DrawScrollCards(int maxScrollDraw = 4)
    {
        int count = Mathf.Min(scrollCards.Count, maxScrollDraw);

        for (int i = 0; i < count; i++)
        {
            var card = scrollCards[i];
            if (card == null) continue;

            var cardUI = cardUIManager.CreateCard(card, handPanel, new Vector2(-800f, -300f));
            cardUI.SetHandView(handView);
            StartCoroutine(handView.AddCard(cardUI));
        }
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
            var cardUI = cardUIManager.CreateCard(card, handPanel, new Vector2(-800f, -300f));
            cardUI.SetHandView(handView);
            StartCoroutine(handView.AddCard(cardUI));
        }
    }

    public void AddCardUI(CardData card)
    {
        var cardUI = cardUIManager.CreateCard(card, handPanel, new Vector2(-800f, -300f));
        cardUI.SetHandView(handView);
        StartCoroutine(handView.AddCard(cardUI));
    }

    //처음을 제외하고 다시 자신의 턴이 되면 카드를 드로우함
    public void ReDrawCards()
    {
        ClearDiscardDeck(); //버림존에 있는 카드를 전부 덱에 넣음
        drawPile.Shuffle();
        //스크롤 카드 먼저 드로우
        StartCoroutine(DrawOpeningHand()); 
        handView.CheckUsableCard();
    }
    
    //카드를 텀을 두고 드로우하는 함수
    public IEnumerator DrawOpeningHand(float delay = 0.2f)
    {
        // 스크롤 카드 드로우
        DrawScrollCards();

        yield return new WaitForSeconds(0.3f);
        // 일반 카드 드로우
        for (int i = 0; i < firstDraw; i++)
        {
            DrawCard();
            yield return new WaitForSeconds(delay);
        }

        handView.CheckUsableCard();
    }

    private void ClearDiscardDeck()
    {
        if (discardPile.cards.Count > 0)
        {
            drawPile.cards.AddRange(discardPile.cards);
            discardPile.cards.Clear();
        }
    }


    /*
    --- test ---
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))  // 스페이스 키 눌렀을 때
        {
            DrawOneCardFromDeckTop();
        }
    
        if (Input.GetKeyDown(KeyCode.E))  // E 키를 눌렀을 때
        {
            handView.DiscardAllCards();
            Debug.Log("DiscardAllCards 실행됨");
        }
    }

    private void DrawOneCardFromDeckTop()
    {
        if (drawPile.cards.Count == 0)
        {
            Debug.Log("DrawPile is empty");
            return;
        }

        var card = drawPile.cards[0];

        var ui = cardUIManager.CreateCard(card, handPanel,new Vector2(-800f,-300f));
        ui.SetHandView(handView);
        StartCoroutine(handView.AddCard(ui));
    }
    */  

}
