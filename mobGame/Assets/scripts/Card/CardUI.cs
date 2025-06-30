using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CardUI : MonoBehaviour
{
    //public Image backgroundImage; //배경 버튼에서 처리
    public Image cardImage;  //카드 일러스트
    public LocalizedText nameText;
    public LocalizedText descriptionText;
    public Button cardButton;

    //private CardData data; 카드 데이터는 나중에 manager가 관리하자

    public void SetCard(CardData newData)
    {
        //TODO 카드 데이터로 저걸 채움
    }
}
