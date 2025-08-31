using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MapSceneManager : MonoBehaviour
{
    public UImanager uimanager;
    //ButtonManager이랑 간접 연결

    [SerializeField] private Image background;

    void Start()
    {
        //TODO currentMap 데이터를 게임 매니저로부터 받아옴 
        MapNode currentMap = GameManager.gameManager.playerData.currentMap;

        //백그라운드 로드
        background.sprite = BackgroundLoader.LoadBackgroundSprite(currentMap.stageNumber, true);

        uimanager.InitMap(currentMap);
    }

    public void HandleStart()
    {
        //TODO uimanager selectedIndex값에 따라 다음 씬으로 행동
        NodeType? nodeType = uimanager.GetNodeType();

        if (nodeType == null)
        {
            Debug.LogWarning("[Battle] 선택된 노드가 없습니다.");
            return;
        }

        GameManager.gameManager.nodeId = uimanager.SelectedIdx;

        switch (nodeType)
        {
            case NodeType.Battle:
                SceneManager.LoadScene("BattleScene");
                //SceneManager.LoadScene("Shop");
                //SceneManager.LoadScene("BattleScene");
                break;

            case NodeType.Shop:
                SceneManager.LoadScene("Shop");
                break;

            case NodeType.Campfire:
                SceneManager.LoadScene("Campfire");
                break;

            case NodeType.Treasure:
                SceneManager.LoadScene("BattleScene");
                break;

            case NodeType.Elite:
                SceneManager.LoadScene("BattleScene");
                break;

            case NodeType.Unknown:
                //TODO 랜덤 노드 이동
                break;

            case NodeType.Boss:
                SceneManager.LoadScene("BattleScene");
                break;
        }
    }


}