using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public enum SceneType
{
    ShopScene, InventoryScene, UpgradeScene
}

public class CardViewerSceneManager : MonoBehaviour, IPushable
{
    [SerializeField] private Transform cardParent;
    [SerializeField] private CardUIManager cardUIManager;

    [SerializeField] private Transform BtnParent;
    [SerializeField] private Button deleteBtn;

    [Header("Layout Settings")]
    [SerializeField, Range(1, 10)]
    private int cardsPerRow = 5;


    private Vector2 firstRowStart = new Vector2(-750f, 200f);

    private Vector2 firstRowEnd = new Vector2(520f, 200f);

    private Vector2 secondRowPos = new Vector2(-750f, -240f);


    void Start()
    {
        List<CardData> allCards = GameManager.gameManager.CardViewCards.cards;

        // 1) 가로·세로 간격 계산
        float xSpacing = (firstRowEnd.x - firstRowStart.x) / (cardsPerRow - 1);
        float ySpacing = firstRowStart.y - secondRowPos.y;

        // 2) 총 행 수
        int totalRows = Mathf.CeilToInt(allCards.Count / (float)cardsPerRow);

        SceneType sceneType = GameManager.gameManager.CardViewCards.sceneType;

        // 3) 카드 생성 & 배치
        for (int i = 0; i < allCards.Count; i++)
        {
            int row = i / cardsPerRow;   // 0부터 시작
            int col = i % cardsPerRow;   // 0 ~ cardsPerRow-1

            float xPos = firstRowStart.x + col * xSpacing;
            float yPos = firstRowStart.y - row * ySpacing;

            if (sceneType == SceneType.InventoryScene)
            {
                cardUIManager.CreateCard(
                    allCards[i],
                    cardParent,
                    new Vector2(xPos, yPos)
                );
            }
            else
            {
                cardUIManager.Register(allCards[i], cardParent, new Vector2(xPos, yPos));
            }

        }

        if (sceneType == SceneType.InventoryScene)
        {
            BtnParent.gameObject.SetActive(true);
            deleteBtn.gameObject.SetActive(false);
        }
        else
        {
            BtnParent.gameObject.SetActive(false);
            deleteBtn.gameObject.SetActive(true);
        }

        deleteBtn.onClick.AddListener(DeleteCard);

    }

    //강화랑 삭제 둘다 같은 역할을 지님(카드 처리)
    public void DeleteCard()
    {
        int selectedIdx = cardUIManager.selectedIdx;
        SceneType sceneType = GameManager.gameManager.CardViewCards.sceneType;

        //선택된 카드가 없을경우 아무것도 하지않고 바로 씬으로 넘김
        if (selectedIdx == -1)
        {
            if (sceneType == SceneType.ShopScene)
            {
                SceneManager.LoadScene("Shop");
            }
            else if (sceneType == SceneType.UpgradeScene)
            {
                SceneManager.LoadScene("Campfire");
            }
            return;
        }

        if (sceneType == SceneType.ShopScene)
        {
            var card = cardUIManager.handCards[selectedIdx];

            GameManager.gameManager.playerData.playerDeck.cards.Remove(card.data); // 카드 제거

            SceneManager.LoadScene("Shop");
        }
        else if (sceneType == SceneType.UpgradeScene)
        {
            var card = cardUIManager.handCards[selectedIdx];

            if (card != null)
            {
                UpgradeProcessor upgradeProcessor = new UpgradeProcessor();
                upgradeProcessor.UpgradeCard(card.data);  // 카드 업그레이드
            }

            SceneManager.LoadScene("Campfire");
        }
    }

    public void PushBtnBack()
    {
        SceneType sceneType = GameManager.gameManager.CardViewCards.sceneType;

        if (sceneType == SceneType.InventoryScene)
        {
            SceneManager.LoadScene("stageScene");
        }
    }
    public void PushBtnInventory() { }
}
