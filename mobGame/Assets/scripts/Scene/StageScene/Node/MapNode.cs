using System;
using System.Collections.Generic;
using UnityEngine;

//맵 테마 (Nop :: 튜토리얼 존)
public enum MapTheme
{
    NOP, FROST, FOREST, OCEAN, DESERT, VOLCANO, RUINS, VOID
};

[Serializable]
public class MapNode
{
    public int stageNumber;
    public MapTheme theme;
    public List<MapNodeData> nodes = new();

    //맵의 모든 요소가 클리어 되었으면 참을 넘김 >> 다음 스테이지 이동시 사용
    public bool IsAllCleared()
    {
        if (nodes == null || nodes.Count == 0)
            return false;

        var lastNode = nodes[^1]; // 리스트의 마지막 요소
        return lastNode != null && lastNode.nodeType == NodeType.Cleared;
    }

    //제일 많이 클리어한 노드 반환시킴
    public int GetMaxClearedNode()
    {
        if (nodes == null || nodes.Count == 0) return -1;
        if (nodes[0] == null) return -1;

        // 0번 노드에서 시작
        var lastCleared = FindLastClearedNode(this, nodes[0]);
        if (lastCleared == null) return -1;

        return lastCleared.nodeId;
    }

    private MapNodeData FindLastClearedNode(MapNode map, MapNodeData start)
    {
        MapNodeData current = start;

        while (true)
        {
            MapNodeData nextCleared = null;

            foreach (int nextId in current.connectedNodeIds)
            {
                var node = map.nodes[nextId];
                if (node != null && node.nodeType == NodeType.Cleared)
                {
                    nextCleared = node;
                    break; // 여러 개면 첫 번째 Cleared만 선택
                }
            }

            if (nextCleared == null)
            {
                return current; // 더 이상 갈 곳 없으면 현재가 마지막 Cleared
            }

            current = nextCleared;
        }
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
