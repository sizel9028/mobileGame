using UnityEngine;
using UnityEngine.UI;

public class CardReward : MonoBehaviour
{
    public GameObject rewardPanel;  
    [SerializeField] private CardUIManager cardUIManager;
    [SerializeField] private Transform cardParent;
    public Button takeRewardBtn;  // 보상 받는 버튼
    public Button skipRewardBtn;  // 보상 스킵 버튼

    public LocalizedText takeRewardTxt;
    public LocalizedText skipRewardTxt;

    public Button resetBtn; // 카드 리셋

    private int maxReset;

    private Vector2[] cardPos = new Vector2[]
    {
        new Vector2(-300f,0f), new Vector2(0f,0f), new Vector2(300f,0f)
    };

    void Start()
    {
        takeRewardBtn.onClick.AddListener(OnTakeReward);
        skipRewardBtn.onClick.AddListener(OnSkipReward);
        resetBtn.onClick.AddListener(OnResetReward);

        //TODO maxReset의 값을 받아옴(계수 모음집에서)
    }

    public void SetActive(bool active)
    {
        rewardPanel.SetActive(active);
        if (active) ShowCard();
    }

    public void ShowCard()
    {
        //TODO 랜덤 카드 3장을 뽑음(따로 만들어야 함)
        //  --- Test ---
        var deck = CardGenerator.LoadDeck(0, 0, 0);
        var card = deck.cards[0];
        //카드는 테스트 용으로 보여줌

        for (int i = 0; i < 3; ++i)
        {
            cardUIManager.Register(card, cardParent, cardPos[i]);
        }

    }

    private void OnResetReward()
    {
        if (maxReset > 0)
        {
            ShowCard();
            --maxReset;
        }
    }

    private void OnTakeReward()
    {
        //TODO 보상 카드를 플레이어 덱에 추가
    }

    private void OnSkipReward()
    {
        //TODO 씬을 바로 넘김 (저장을 하고 넘길지 아니면 맵에서 저장할지) 아마 맵에서 저장할듯
    }
}