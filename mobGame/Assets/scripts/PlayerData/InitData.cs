
using UnityEngine;

public static class InitData
{
    public static PlayerData CreateNewPlayerData()
    {
        return new PlayerData
        {
            //TODO 초기값 설정
            gold = 50,
            DecreaseMaxHp = 0,
            currentMap = MapGenerator.LoadMap(0, 0, 0),  //튜토리얼 맵
            playerDeck = null,
            characterData = null,
            difficulty = 0
        };
    }

    public static RuneData CreateNewRuneData()
    {
        RuneData runeData = new RuneData();

        foreach (MapTheme theme in System.Enum.GetValues(typeof(MapTheme)))
        {
            Rune rune = new Rune
            {
                level = 0,
                mapTheme = theme
            };
            runeData.runes.Add(rune);
        }

        return runeData;
    }
}
