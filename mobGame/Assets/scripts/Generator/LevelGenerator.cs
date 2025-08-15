using UnityEngine;


//레벨을 만드는 클래스
public static class LevelGenerator
{
    //stage에 존재하는 총 난이도 갯수(스테이지에 존재하는 몬스터 숫자) (1~5은 일반,6~7은 엘리트, 8은 보스)
    private const int TotalBattleLevels = 5; // 일반 몬스터 레벨 1~5
    private const int TotalEliteLevels = 2; // 엘리트 레벨 6~7
    private const int TotalBossLevels = 1; // 보스 레벨 8

    private const int EliteStartLevel = TotalBattleLevels + 1; // 6
    private const int BossLevel = TotalBattleLevels + TotalEliteLevels + 1; // 8

    public static int GetLevelInfo(int nodeId)
    {
        var currentMap = GameManager.gameManager.playerData.currentMap;
        int stageNumber = currentMap.stageNumber;
        int themeIdx = (int)currentMap.theme;

        int currentMapLevel = nodeId;  //현재 진행노드
        int totalMapLevel = currentMap.GetTotalNode();  // 토탈 노드

        var nodeType = currentMap.nodes[nodeId].nodeType;

        if (nodeType == NodeType.Boss) return BossLevel; //최고 난이도

        if (nodeType == NodeType.Elite)
        {
            int eliteSeed = stageNumber + 10 * themeIdx + 1000;
            System.Random eliteRng = new System.Random(eliteSeed + nodeId);
            return eliteRng.Next(EliteStartLevel, EliteStartLevel + TotalEliteLevels);
        }


        float progressRatio = totalMapLevel == 0 ? 0f : (float)currentMapLevel / totalMapLevel;

        int baseLevel = (int)(progressRatio * TotalBattleLevels);

        int Gold = GameManager.gameManager.playerData.gold * 3;
        int seed = stageNumber + 10 * themeIdx + 100 * Gold;
        System.Random rng = new System.Random(seed + nodeId);
        int offset = rng.Next(-1, 2);

        int finalLevel = Mathf.Clamp(baseLevel + offset + 1, 1, TotalBattleLevels); //마지막 레벨은 보스용도
        return finalLevel;
    }
}
