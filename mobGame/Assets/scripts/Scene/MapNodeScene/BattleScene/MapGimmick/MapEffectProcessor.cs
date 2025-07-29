using UnityEngine;

//맵이 가지는 기믹 처리
public class MapEffectProcessor : Singleton<MapEffectProcessor>
{

    public void ProcessMapEffect()
    {
        MapTheme mapTheme = GameManager.gameManager.playerData.currentMap.theme;
        int turnCount = Battle.Instance.turnCount; //현재 턴
        switch (mapTheme)
        {
            case MapTheme.FROST:
                break;
        }
    }

}
