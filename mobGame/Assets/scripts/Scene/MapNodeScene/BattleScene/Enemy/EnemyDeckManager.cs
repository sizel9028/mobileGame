using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//에너미의 덱을 관리하는 매니저
public class EnemyDeckManager : Singleton<EnemyDeckManager>
{
    private Deck deck = new();
    public List<CardData> passiveCards = new();
    private List<CardData> handCards = new();
    private EnemyAI enemyAI = new();

    //시뮬레이션 루트
    [SerializeField] private Transform simRoot;

    public void InitEnemyDeck()
    {
        var currMap = GameManager.gameManager.playerData.currentMap;
        //int[] levelInfo = LevelGenerator.GetLevelInfo(currMap);
        int level = LevelGenerator.GetLevelInfo(GameManager.gameManager.nodeId);
        //deck = CardGenerator.LoadDeck(levelInfo[0], levelInfo[1], levelInfo[2]); //덱 로드
        //--- test ---
        var map = GameManager.gameManager.playerData.currentMap;
        deck = CardGenerator.LoadDeck(map.stageNumber, (int)map.theme, level);

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

    //amount만큼 덱에서 카드를 드로우 하고 handCards에 넣음
    private void DrawCards(int amount)
    {
        //먼저 덱을 셔플 + 핸드 카드 초기화
        handCards.Clear();
        deck.Shuffle();

        for (int i = 0; i < amount; i++)
        {
            if (deck.cards.Count == 0) break;

            CardData drawnCard = deck.PeekDraw(i);

            if (drawnCard != null)
            {
                handCards.Add(drawnCard);
                Debug.Log($"[EnemyDeckManager] 무작위 카드 확인: {drawnCard.nameKey}");
            }
        }
    }

    public IEnumerator PlayCard()
    {
        DrawCards(5);
        enemyAI.SetCards(handCards, simRoot);  //패에 있는 카드 셋팅
        yield return StartCoroutine(enemyAI.PlayCard());

        Debug.Log("PlayCard 정상작동 완료");
    }
}
