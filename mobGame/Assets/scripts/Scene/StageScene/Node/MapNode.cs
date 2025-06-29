using System.Collections.Generic;
using UnityEngine;

//맵 테마
public enum MapTheme
{
    FROST
};

public class MapNode
{
    public int stageNumber;
    public MapTheme theme;
    public MapNodeData[] nodes = new MapNodeData[50];

    //맵의 모든 요소가 클리어 되었으면 참을 넘김 >> 다음 스테이지 이동시 사용
    public bool isAllCleared()
    {
        foreach (var node in nodes)
        {
            if (node == null) continue;
            if (node.nodeType != NodeType.Cleared)
                return false;
        }

        return true;
    }
}
