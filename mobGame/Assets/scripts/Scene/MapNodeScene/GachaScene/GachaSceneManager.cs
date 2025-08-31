using TMPro;
using UnityEngine;

public class GachaSceneManager : MonoBehaviour
{
    [Header("clear UI")]
    public TMP_Text clearText;  // Clear보여줌
    //얼만큼의 재화가 증가했는지
    public TMP_Text goldText;
    public GameObject infoPanel;  // 텍스트 모아둔 상위 오브젝트
    public GameObject buttons;
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
        int gold = GetGold();
        goldText.text = $"gold : +{gold}";

        GameManager.gameManager.playerData.gold += gold;
    }


    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            infoPanel.SetActive(false);
            cardReward.SetActive(true);
            buttons.SetActive(true);

            this.enabled = false; //업데이트 종료
        }
    }

    private int GetGold()
    {
        var map = GameManager.gameManager.playerData.currentMap;
        int stageNumber = map.stageNumber;
        var nodeId = GameManager.gameManager.nodeId;
        NodeType nodeType = map.nodes[nodeId].nodeType;

        int baseGold = 0;

        switch (nodeType)
        {
            case NodeType.Battle:
                baseGold = 10;
                break;
            case NodeType.Elite:
                baseGold = 30;
                break;
            case NodeType.Boss:
                baseGold = 60;
                break;
            case NodeType.Treasure:
                baseGold = 25;
                break;
            default:
                baseGold = 0;
                break;
        }

        float min = baseGold * (stageNumber+1) * 0.5f;
        float max = baseGold * (stageNumber+1) * 1.5f;

        int gold = Mathf.RoundToInt(Random.Range(min, max));

        return gold;
    }
}
