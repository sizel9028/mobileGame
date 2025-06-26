using UnityEngine;

public class MapSceneManager : MonoBehaviour
{
    public UImanager uimanager;
    //prefab필요
    private MapNode currentMap;

    void Start()
    {
        //TODO currentMap 데이터를 게임 매니저로부터 받아옴
        uimanager.InitMap(currentMap);
    }


}