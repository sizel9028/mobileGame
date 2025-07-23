using UnityEngine;

public class InitData : MonoBehaviour
{
    public static PlayerData CreateNewPlaterData()
    {
        CharacterData testData = new();
        testData.name = "Slime";
        testData.maxHp = 50;
        testData.hp = 20;
        testData.baseShield = 0;
        return new PlayerData
        {
            //TODO 초기값 설정
            gold = 0,
            currentMap = MapGenerator.LoadMap(1, 0),
            playerDeck = CardGenerator.LoadDeck(0, 0, 0),
            characterData = testData
        };
    }
}
