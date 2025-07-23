using UnityEngine;

public static class LevelGenerator 
{
    public static int[] GetLevelInfo(MapNode currentMap)
    {
        int stageNumber = currentMap.stageNumber;
        int themeIdx = (int)currentMap.theme;

        int currentMapLevel = currentMap.GetMaxClearedNode();
        int totalMapLevel = currentMap.GetTotalNode();

        int totalLevel = 7;  //stage에 존재하는 총 난이도 갯수

        float progressRatio = totalMapLevel == 0 ? 0f : (float)currentMapLevel / totalMapLevel;

        int baseLevel = (int)(progressRatio * totalLevel);

        int Gold = GameManager.gameManager.playerData.gold * 3;
        int seed = stageNumber + 10 * themeIdx + 100*Gold;
        System.Random rng = new System.Random(seed);
        int offset = rng.Next(-1, 2);

        int finalLevel = Mathf.Clamp(baseLevel + offset, 1, totalLevel - 1); //마지막 레벨은 보스용도
        return new int[3]
        {
            stageNumber, themeIdx, finalLevel
        };
    }
}
