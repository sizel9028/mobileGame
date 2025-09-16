using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CardUI : MonoBehaviour
{
    //public Image backgroundImage; //배경 버튼에서 처리
    //private GameObject wrapper; // 상위 빈 객체
    public Image cardImage;  //카드 일러스트
    public LocalizedText nameText;
    public LocalizedText descriptionText;
    public LocalizedText typeText; //카드가 어떤 타입인지
    public Image cardBackImage;  //카드 전반적인 틀
    public Image borderImage;   //카드 테두리(색+선택 상호작용)

    public TMP_Text costText;  //코스트 텍스트

    //private CardData data; 카드 데이터는 나중에 manager가 관리하자
    //private int index = -1; // 카드 자신이 가지는 번호 (리스트 처리)
    public CardData data;

    //2개의 관리 매니저(handView :: 패에 있는 카드, manager : 이외의 상황)
    private CardUIManager manager;
    private HandView handView;
    private ShopView shopView; // 인터페이스 쓸걸...

    private static readonly Color[] rareColors = new Color[]
    {
        //TODO 색 채우기 rare 별로 (회색,녹색,블루,보라,빨강)
        new Color(0.8f, 0.8f, 0.8f), new Color(0.0f, 0.39f, 0.0f), new Color(0.0f, 0.0f, 0.55f),
        new Color(0.5f, 0f, 0.5f), new Color(0.55f, 0.0f, 0.0f)
    };

    //카드 정보를 변경 하고, 그 카드 정보의 UI를 업데이트함
    public void Setup()
    {
        nameText.SetText(data.nameKey);
        SetDescText(data.descriptionKey, data);
        costText.text = data.cost.ToString();
        SetTypeText(data.cardType);
    }

    public void SetCard(CardData newData, CardUIManager uiManager)
    {
        data = newData;
        manager = uiManager;
        //TODO 카드 데이터로 저걸 채움
        nameText.SetText(newData.nameKey);
        //descriptionText.SetText(newData.descriptionKey);  // key를 바탕으로 값 결정
        SetDescText(newData.descriptionKey, newData);
        costText.text = newData.cost.ToString();
        SetTypeText(newData.cardType);

        StartCoroutine(CardArtLoader.LoadCardArt(newData, (sprite) =>
        {
            if (sprite != null)
            {
                cardImage.sprite = sprite;
            }
            else
            {
                Debug.LogError($"[CardUI] 카드 아트 로딩 실패: {newData.cardArtName}");
            }
        }));

        if (borderImage != null)
            borderImage.color = rareColors[(int)newData.rare];

    }

    private void SetTypeText(CardType type)
    {
        switch (type)
        {
            case CardType.Passive:
                typeText.SetText("passive_type");
                break;

            case CardType.Action:
                typeText.SetText("action_type");
                break;

            case CardType.Scroll:
                typeText.SetText("scroll_type");
                break;

            default:
                break;
        }
    }
    //분노 상태일때 카드 아트바뀜
    public void ReloadArt()
    {
        if (data == null)
        {
            Debug.LogError("[CardUI] 데이터가 없어 아트 재로딩 불가");
            return;
        }

        StartCoroutine(CardArtLoader.LoadCardArt(data, (sprite) =>
        {
            if (sprite != null)
            {
                cardImage.sprite = sprite;
                Debug.Log($"[CardUI] 카드 아트 재로딩 성공: {data.cardArtName}");
            }
            else
            {
                Debug.LogError($"[CardUI] 카드 아트 재로딩 실패: {data.cardArtName}");
            }
        }));
    }


    private void SetDescText(string key, CardData cardData)
    {
        string rawText = LocalizationManager.languageM.GetText(key);

        foreach (var kvp in cardData.effectMap)
        {
            string placeholder = "{" + kvp.Key + "}"; // Damage >> {Damage} 로 바꿈
            float fVal = kvp.Value;

            string value;
            
            string percentPlaceholder = "[" + kvp.Key + "]";

            if (rawText.Contains(percentPlaceholder))
            {
                Debug.Log(cardData.cardArtName + "발견됨");
                // 예: 0.03 -> 3
                value = Mathf.RoundToInt(fVal * 100).ToString();
                rawText = rawText.Replace(percentPlaceholder, value);
            }
            
            if (Mathf.Abs(fVal % 1) < 0.001f)
            {
                // 정수면 소수점 없이
                value = ((int)fVal).ToString();
            }
            else
            {
                // 소수면 소수점 둘째 자리까지
                value = fVal.ToString("0.##");
            }

            rawText = rawText.Replace(placeholder, value);
        }

        descriptionText.Clear();
        descriptionText.AppendText(rawText);
    }

    //이 카드의 하위 오브젝트의 raycast를 전부 꺼버림
    public void SetRaycastFalse()
    {
        Graphic[] graphics = GetComponentsInChildren<Graphic>(true);

        foreach (var g in graphics)
        {
            g.raycastTarget = false;
        }
    }

    public void SetSelected(bool isSelected)
    {
        //TODO 선택되면 변화가 있어야함
        if (borderImage == null) return;

        borderImage.color = isSelected ? new Color(1f, 0.95f, 0.4f) : rareColors[(int)data.rare];
    }

    public void SetManager(CardUIManager uiManager)
    {
        this.manager = uiManager;
    }

    // 터치 관련 함수 (패에 있는 카드들은 handManager한테 넘김)
    public void SetHandView(HandView manager)
    {
        handView = manager;
        this.manager = null; // cardUImanager과의 연결을 끊음 (무조건 패로만 사용함)
    }

    public void SetShopView(ShopView shopView)
    {
        this.shopView = shopView;
        this.manager = null;
        this.handView = null;
    }

    public void OnPointerDown(BaseEventData data)
    {
        //Debug.Log("Button Pressed: " + gameObject.name);  // 현재 버튼 이름 출력
        manager?.OnCardClick(this);  // 자신을 넘겨서 인덱스를 사용x
        handView?.OnCardDown(this);
        shopView?.OnCardDown(this);
    }

    // 버튼에서 손을 뗐을 때 호출
    public void OnPointerUp(BaseEventData data)
    {
        //Debug.Log("Button Released: " + gameObject.name);  // 현재 버튼 이름 출력
        handView?.OnCardUp(this);
        shopView?.OnCardUp(this);
    }

    public void UpdateUsableVisual()
    {
        bool usable = CardValidator.IsCardAble(data, true);
        Color color = usable ? Color.white : new Color(0.5f, 0.5f, 0.5f, 1f);

        cardImage.color = color;
        cardBackImage.color = color;
        borderImage.color = usable ? rareColors[(int)data.rare] : color;

        Debug.Log($"[CardUI] 카드: {data.nameKey}, 비용: {data.cost}, 사용 가능 여부: {usable}");
    }

    public void DestroyCard()
    {
        transform.DOKill(true);

        Destroy(gameObject);
    }


}
