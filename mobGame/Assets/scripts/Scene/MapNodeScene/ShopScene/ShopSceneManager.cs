using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ShopSceneManager : Singleton<ShopSceneManager>, IPushable
{
    [SerializeField] private CardUIManager cardUIManager;
    [SerializeField] private ShopView shopView;
    [SerializeField] private Transform cardParent;
    [SerializeField] private List<RectTransform> cardSlots;

    [SerializeField] private Image descBackImage;
    [SerializeField] private LocalizedText descText;  //상점 주인 텍스트(가격 포함)
    [SerializeField] private Button buyBtn;

    [SerializeField] private TextMeshProUGUI goldText; // 현재 골드

    private CardCostGenerator cardCostGenerator = new();
    private CardDataGenerator cardDataGenerator = new();

    public int currentGold = 0;

    void Start()
    {
        currentGold = GameManager.gameManager.playerData.gold;
        SetGoldTxt();
        buyBtn.onClick.AddListener(PushBtnBuy);
        //배경 투명도
        SetDescBackgroundTransparency();

        if (GameManager.gameManager.shopSceneCards != null)
        {
            descText.SetText("shop_sale");
            var cards = GameManager.gameManager.shopSceneCards;
            for (int i = 0; i < cards.Count; ++i)
            {
                AddCardToShop(cards[i], i);
            }
            GameManager.gameManager.shopSceneCards = null; // 카드 제거 후 다시 돌아올때
            return;
        }

        SetStartDescText(); // 초기 대화 생성
                            // --- test ---
        var deck = cardDataGenerator.GenerateCard();
        var card = cardDataGenerator.MkDeleteCard();
        //Debug.Log(deck.Count);

        for (int i = 0; i < cardSlots.Count; ++i)
        {
            if (i == 9)
            { // --- test ---
                AddCardToShop(card, i);
                break;
            }
            AddCardToShop(deck[i], i);
        }

    }

    private void SetGoldTxt()
    {
        int gold = GameManager.gameManager.playerData.gold;
        goldText.text = $"{gold}g";
    }
    
    private void AddCardToShop(CardData cardData, int slotIdx)
    {
        CardUI ui = cardUIManager.CreateCard(cardData, cardParent, cardSlots[slotIdx].anchoredPosition);
        ui.transform.localScale = Vector3.one * 2f;
        ui.SetShopView(shopView);
        shopView.AddCard(ui);
    }

    //알파값 조절해서 투명하게 함
    private void SetDescBackgroundTransparency()
    {
        if (descBackImage != null)
        {
            Color color = descBackImage.color;
            color.a = 0.4f;
            descBackImage.color = color;
        }
    }

    private void SetStartDescText()
    {
        int random = UnityEngine.Random.Range(1, 8); // 7개의 텍스트

        string key = $"shop_random_{random}";

        descText.Clear();
        descText.SetText(key);
    }

    //가격 표시
    public void SetDescText()
    {
        var ui = shopView.GetSelectedCard();  //셀렉트 카드 얻음
        var cardData = ui.data;

        int gold = cardCostGenerator.GetCost(cardData);
        descText.Clear();
        descText.AppendText($"{gold}G");
    }

    //구매후 보여줄 텍스트
    public void SetBuyText()
    {
        int random = UnityEngine.Random.Range(1, 6); // 5개의 텍스트

        string key = $"shop_buyrandom_{random}";

        descText.Clear();
        descText.SetText(key);
    }

    public void PushBtnBuy()
    {
        var card = shopView.GetSelectedCard();
        if (card == null) return;

        int cost = cardCostGenerator.GetCost(card.data);
        var player = GameManager.gameManager.playerData;

        if (player.gold < cost)
        {
            //TODO 잔액없다는 텍스트 표시
            descText.SetText("shop_nomoney");
            return;
        }

        player.gold -= cost;

        if (card.data.nameKey == "shop_delete_card")
        {
            GameManager.gameManager.buyDeleteCard++;
            GameManager.gameManager.CardViewCards = (SceneType.ShopScene, GameManager.gameManager.playerData.playerDeck.cards);
            //shopView.RemoveSelectedCard();

            List<CardData> cardDataLists = shopView.cards.Select(card => card.data).ToList();
            GameManager.gameManager.shopSceneCards = cardDataLists;


            SceneManager.LoadScene("CardViewerScene");
            return;
        }
        player.playerDeck.cards.Add(card.data);

        shopView.RemoveSelectedCard();

        SetBuyText();
        SetGoldTxt();
    }


    public void PushBtnBack()
    {
        //TODO 화면 나가고 맵 저장
        SaveManager.saveManager.SaveAll();
        GameManager.gameManager.buyDeleteCard = 0;

        SceneManager.LoadScene("stageScene");
    }
    public void PushBtnInventory(){}
}
