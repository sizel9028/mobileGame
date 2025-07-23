using System.Collections.Generic;
using UnityEngine;

//에너미의 덱을 관리하는 매니저
public class EnemyDeckManager : Singleton<EnemyDeckManager>
{
    private Deck deck = new();
    private List<CardData> passiveCards = new();
    private List<CardData> handCards = new();

    public void InitEnemyDeck()
    {
        var currMap = GameManager.gameManager.playerData.currentMap;
        int[] levelInfo = LevelGenerator.GetLevelInfo(currMap);

        deck = CardGenerator.LoadDeck(levelInfo[0], levelInfo[1], levelInfo[2]); //덱 로드

        passiveCards.Clear();

        //패시브 카드를 패시브 카드 존에 넣고, 덱에서 제거시킴
        for (int i = deck.cards.Count - 1; i >= 0; --i)
        {
            CardData card = deck.cards[i];
            if (card.cardType == CardType.Passive)
            {
                passiveCards.Add(card);

                deck.cards.RemoveAt(i);
            }
        }
    }
}
