using UnityEngine;

public static class SeedManager
{
    public static System.Random GetNodeRng(int stageIdx, string cardName)
    {
        int baseSeed = GameManager.gameManager.playerData.seed;
        int cardHash = cardName.GetHashCode();
        int nodeId = GameManager.gameManager.playerData.currentMap.GetMaxClearedNode();
        int nodeSeed = baseSeed + 1000 * stageIdx + 100 * nodeId + cardHash;
        return new System.Random(nodeSeed);
    }
}
