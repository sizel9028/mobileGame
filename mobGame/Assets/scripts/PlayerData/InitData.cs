using UnityEngine;

public class InitData : MonoBehaviour
{
    public static PlayerData CreateNewPlaterData()
    {
        return new PlayerData
        {
            //TODO 초기값 설정
            hp = 100,
            gold = 0,
            currentMap = MapGenerator.GenerateStage1()
        };
    }
}
