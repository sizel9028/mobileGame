using System.Collections.Generic;
using UnityEngine;

public class CardUIManager : MonoBehaviour
{
    public List<CardUI> cardUIs = new();

    public void Register(CardUI card, int index)
    {
        card.SetIndex(index);
        card.SetManager(this);
        cardUIs.Add(card);
    }

    public void OnCardClick(int index)
    {
        //TODO 카드 클릭 정보를 넘김, 드래그 기능 추가
    }
}
