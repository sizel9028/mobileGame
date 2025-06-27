using UnityEngine;

public class MapGenerator : MonoBehaviour
{
    public static MapNode GenerateStage1()
    {
        MapNode map = new MapNode();
        map.stageNumber = 1;
        map.theme = MapTheme.FROST; //TODO 초기 맵 값으로 이후 변경

        //TODO 맵 값 추후 입력(현재 예시 맵)
        for (int i = 0; i < 5; i++)
        {
            MapNodeData node = new MapNodeData();
            node.nodeId = i;
            node.nodeType = NodeType.Battle;  
            node.position = new Vector2(i * 50, 0); 

            // 연결 예시
            if (i > 0)
                node.connectedNodeIds.Add(i - 1);
            if (i < 4)
                node.connectedNodeIds.Add(i + 1);

            map.nodes[i] = node;
        }

        return map;
    }
}
