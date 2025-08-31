using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
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

    private int maxReset = 2;
    private RewardGenerator rewardGenerator = new();

    private Vector2 defaultCardPos = new Vector2(0f, 800f);  //카드가 생성되는 시작 위치

    private Vector2[] cardPos = new Vector2[]
    {
        new Vector2(-400f,0f), new Vector2(0f,0f), new Vector2(400f,0f)
    };

    void Start()
    {
        takeRewardBtn.onClick.AddListener(OnTakeReward);
        skipRewardBtn.onClick.AddListener(OnSkipReward);
        resetBtn.onClick.AddListener(OnResetReward);

        //TODO maxReset의 값을 받아옴(계수 모음집에서)
        maxReset += GameManager.gameManager.maxReset;
    }

    public void SetActive(bool active)
    {
        rewardPanel.SetActive(active);
        if (active) StartCoroutine(ShowCard());
    }

    private IEnumerator ShowCard()
    {
        resetBtn.interactable = false;
        skipRewardBtn.interactable = false;
        takeRewardBtn.interactable = false;
        //TODO 랜덤 카드 3장을 뽑음(따로 만들어야 함)
        //  --- Test ---
        //var deck = CardGenerator.LoadDeck(0, 0, 0);
        //var card = deck.cards[0];
        //카드는 테스트 용으로 보여줌
        var rewards = rewardGenerator.GetRewardCard();

        if (rewards == null || rewards.Count < 3)
        {
            Debug.LogWarning("[CardReward] 보상 카드 없음");
            yield break;
        }

        for (int i = 0; i < 3; ++i)
        {
            yield return new WaitForSeconds(1f);
            // --- Test ---
            var card = rewards[i];
            // --- Test ---

            var cardUI = cardUIManager.Register(card, cardParent, defaultCardPos);
            var rect = cardUI.GetComponent<RectTransform>();
            rect.localScale = Vector3.one * 4f;
            yield return new WaitForSeconds(0.1f);  //리소스 로딩 시간

            if (rect == null) continue;

            rect.DOAnchorPos(cardPos[i], 0.5f).SetEase(Ease.OutBack); // SetEase 애니메이션 튀기는 효과 + 딜레이
        }

        --maxReset;
        if (maxReset != 0)
        {
            resetBtn.interactable = true;
        }
        takeRewardBtn.interactable = true;
        skipRewardBtn.interactable = true;
    }

    private void OnResetReward()
    {
        if (maxReset > 0)
        {
            cardUIManager.ClearAllCards();
            StartCoroutine(ShowCard());
        }
    }

    private void OnTakeReward()
    {
        //TODO 보상 카드를 플레이어 덱에 추가
        int selected = cardUIManager.selectedIdx;

        if (selected < 0)
        {
            Debug.LogWarning("[CardReward] 선택된 카드가 없습니다");
            return;
        }

        var selectedCard = cardUIManager.handCards[selected];
        //카드를 넣음
        GameManager.gameManager.playerData.playerDeck.cards.Add(selectedCard.data);

        StartCoroutine(MoveMapScene());
    }

    private void OnSkipReward()
    {
        //TODO 씬을 바로 넘김 (저장을 하고 넘길지 아니면 맵에서 저장할지) 아마 맵에서 저장할듯
        StartCoroutine(MoveMapScene());
    }

    private IEnumerator MoveMapScene()
    {
        takeRewardBtn.interactable = false;
        skipRewardBtn.interactable = false;
        resetBtn.interactable = false;
        SaveManager.saveManager.SaveAll();

        yield return new WaitForSeconds(1f);

        if (SaveManager.saveManager.isTutorialEnd)
        {
            //TODO 맵 설명 맵으로 이동
        }

        //saveall에서 게임이 끝나면 이건 작동안함
        UnityEngine.SceneManagement.SceneManager.LoadScene("StageScene");
    }

}