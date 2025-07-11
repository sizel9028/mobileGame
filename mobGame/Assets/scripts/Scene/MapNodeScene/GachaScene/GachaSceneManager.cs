using TMPro;
using UnityEngine;

public class GachaSceneManager : MonoBehaviour
{
    [Header("clear UI")]
    public TMP_Text clearText;  // Clear보여줌
    //얼만큼의 재화 hp가 증가했는지
    public TMP_Text hpText;
    public TMP_Text goldText;
    public GameObject infoPanel;  // 텍스트 모아둔 상위 오브젝트
    public LocalizedText clickToContinueTxt; // 아무곳이나 눌러 처리하기

    //TODO 버튼 추가(카드 보상 받기, 아무 보상 받지 않기)

    public CardReward cardReward;

    void Start()
    {
        cardReward.SetActive(false);
        SetMsg();
    }

    private void SetMsg()
    {
        clearText.text = "Clear";
        //TODO 텍스트 세팅
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            infoPanel.SetActive(false);
            cardReward.SetActive(true);

            this.enabled = false; //업데이트 종료
        }
    }
}
