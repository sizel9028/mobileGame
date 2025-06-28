using UnityEngine;

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
    }

    public void HandleBack()
    {
        //TODO start씬으로 넘김
    }

}