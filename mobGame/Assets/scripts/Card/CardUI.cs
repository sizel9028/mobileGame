using TMPro;
using Unity.Android.Gradle.Manifest;
using UnityEngine;
using UnityEngine.UI;

public class CardUI : MonoBehaviour
{
    //public Image backgroundImage; //배경 버튼에서 처리
    public Image cardImage;  //카드 일러스트
    public LocalizedText nameText;
    public LocalizedText descriptionText;
    public Button cardButton;  //카드 전반적인 틀
    public Image borderImage;   //카드 테두리(색+선택 상호작용)

    public TMP_Text costText;  //코스트 텍스트

    //private CardData data; 카드 데이터는 나중에 manager가 관리하자
    //private int index = -1; // 카드 자신이 가지는 번호 (리스트 처리)
    private CardUIManager manager;

    private static readonly Color[] rareColors = new Color[]
    {
        //TODO 색 채우기 rare 별로
        Color.gray, Color.gray, Color.gray, Color.gray
    };


    public void SetCard(CardData newData, CardUIManager uiManager)
    {
        manager = uiManager;
        //TODO 카드 데이터로 저걸 채움
        nameText.SetText(newData.nameKey);
        descriptionText.SetText(newData.descriptionKey);  // key를 바탕으로 값 결정
        costText.text = newData.cost.ToString();

        if (newData.cardArt != null)
        {
            cardImage.sprite = newData.cardArt;
        }
        else
        {
            Debug.LogWarning($"[CardUI] 카드 아트가 비어있습니다: {newData.cardArtName}");
        }

        if (borderImage != null)
            borderImage.color = rareColors[(int)newData.rare];

        cardButton.onClick.RemoveAllListeners();
        cardButton.onClick.AddListener(OnClick);
    }

    private void OnClick()
    {
        //manager?.OnCardClick(index);
        manager?.OnCardClick(this);  // 자신을 넘겨서 인덱스를 사용x
    }

    public void SetSelected(bool isSelected)
    {
        //TODO 선택되면 변화가 있어야함
        if (borderImage == null) return;

        borderImage.color = isSelected ? Color.white : Color.gray;
    }

}
