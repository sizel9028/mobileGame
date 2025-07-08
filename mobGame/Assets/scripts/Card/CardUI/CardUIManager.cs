using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using UnityEngine;

public class CardUIManager : MonoBehaviour
{
    public GameObject cardPrefab;  //[SerializeField] 어차피 2명인데 다 public으로 넘기자
    public List<CardUI> handCards = new(); // 카드 담음
    private Dictionary<CardUI, CardData> cardLookup = new(); //CardUI를 얻으면 그 CardData를 반환

    private int selectedIndex = -1; //어떤 인덱스가 선택되었는지(리스트 기준)
    //public bool isBattleUI = false; //배틀 UI는 카드 배치를 사용
                                    //public HandView handView; handView는 따로 관리(역할이 다름 패에 보여주게 하는녀석)

    //카드 등록함수 (드로우 패는 HandView에서 관리)
    public void Register(CardData cardData, Transform handParent, Vector2 anchoredPos = default)
    {
        var ui = CreateCard(cardData, handParent, anchoredPos);

        handCards.Add(ui);
        cardLookup[ui] = cardData;
    }

    //카드 프리팹 복제함수
    public CardUI CreateCard(CardData cardData, Transform handParent, Vector2 anchoredPos = default)
    {
        var obj = Instantiate(cardPrefab, handParent);
        var ui = obj.GetComponent<CardUI>();
        ui.SetCard(cardData, this);

        var rt = obj.GetComponent<RectTransform>();
        rt.anchoredPosition = anchoredPos;

        return ui;
    }

    public void OnCardClick(CardUI clickedUI)
    {
        //TODO 카드 클릭 정보를 넘김, 드래그 기능 추가
        int index = handCards.IndexOf(clickedUI);
        if (index == -1)
        {
            Debug.LogWarning("Clicked CardUI not found in handCards");
            return;
        }

        if (index == selectedIndex) return; //같으면 바꿀 필요없음

        if (selectedIndex != -1)
        {
            var fromUI = handCards[selectedIndex];
            fromUI.SetSelected(false);
        }

        var toUI = handCards[index];
        toUI.SetSelected(true);

        selectedIndex = index;

    }

    public void RemoveCard(int index)
    {
        if (index < 0 || index >= handCards.Count)
        {
            Debug.LogWarning($"[CardUIManager] 유효하지 않은 인덱스: {index}");
            return;
        }

        CardUI ui = handCards[index];
        handCards.RemoveAt(index); //카드 리스트에서 제거
        //TODO 카드 제거전 제거 효과 보여주기
        Destroy(ui.gameObject);    //카드 클론 삭제

        if (index == selectedIndex)
        {
            selectedIndex = -1;
        }
        else if (index < selectedIndex)
        {
            --selectedIndex;  // index 보정 리스트 밀림
        }
    }

    public void RemoveSelectedCard()
    {
        RemoveCard(selectedIndex);
    }

}
