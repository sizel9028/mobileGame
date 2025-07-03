using UnityEngine;
using UnityEngine.SceneManagement;

public class MapSceneManager : MonoBehaviour
{
    public UImanager uimanager;
    //ButtonManager이랑 간접 연결

    void Start()
    {
        //TODO currentMap 데이터를 게임 매니저로부터 받아옴 
        MapNode currentMap = GameManager.gameManager.playerData.currentMap;
        
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

        switch (nodeType)
        {
            case NodeType.Battle:
                SceneManager.LoadScene("BattleScene");
                break;
        }
    }

    public void HandleBack()
    {
        //TODO start씬으로 넘김
    }

}