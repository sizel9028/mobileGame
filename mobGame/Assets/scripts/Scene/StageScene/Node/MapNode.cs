using System.Collections.Generic;
using UnityEngine;

//맵 테마 (Nop :: 튜토리얼 존)
public enum MapTheme
{
    NOP, FROST, FOREST, OCEAN, DESERT, VOLCANO, RUINS, VOID
};

public class MapNode
{
    public int stageNumber;
    public MapTheme theme;
    public MapNodeData[] nodes = new MapNodeData[50];

    //맵의 모든 요소가 클리어 되었으면 참을 넘김 >> 다음 스테이지 이동시 사용
    public bool IsAllCleared()
    {
        for (int i = nodes.Length - 1; i >= 0; --i)
        {
            var node = nodes[i];
            if (node == null) continue;

            return node.nodeType == NodeType.Cleared;
        }

        return false;
    }

    //제일 많이 클리어한 노드 반환시킴
    public int GetMaxClearedNode()
    {
        int maxId = -1;

        foreach (var node in nodes)
        {
            if (node == null) continue;

            if (node.nodeType == NodeType.Cleared && node.nodeId > maxId)
            {
                maxId = node.nodeId;
            }
        }

        return maxId;
    }

    public int GetTotalNode()
    {
        int cnt = 0;

        foreach (var node in nodes)
        {
            if (node != null)
            {
                ++cnt;
            }
        }

        return cnt;
    }
}
