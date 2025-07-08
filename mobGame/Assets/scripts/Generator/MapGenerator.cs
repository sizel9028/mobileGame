using System;
using System.Collections.Generic;
using UnityEngine;

public class MapGenerator : MonoBehaviour
{
    // csv파일로 저장되어 있는 맵을 불러옴

    public static MapNode LoadMap(int stage, int mapKind = -1)
    {
        if (mapKind < 0)
        {
            //TODo 각 스테이지별로 맵의 갯수를 선언
            int maxKind = 3;
            mapKind = UnityEngine.Random.Range(0, maxKind);
        }

        string path = $"Maps/Stage{stage}_{mapKind}";
        TextAsset csvFile = Resources.Load<TextAsset>(path);

        if (csvFile == null)
        {
            return null;
        }

        MapNode map = new MapNode();
        map.stageNumber = stage;
        map.theme = MapTheme.FROST;  // 맵 생성후 테마는 기존껄 사용
        map.nodes = new MapNodeData[50];

        string[] lines = csvFile.text.Split('\n');
        for (int i = 1; i < lines.Length; ++i)
        {
            string line = lines[i].Trim();
            if (string.IsNullOrWhiteSpace(line))
                continue;

            string[] parts = line.Split(',');

            int id = int.Parse(parts[0]);
            NodeType type = Enum.Parse<NodeType>(parts[1]);
            float posX = float.Parse(parts[2]);
            float posY = float.Parse(parts[3]);

            List<int> connections = new();
            if (!string.IsNullOrWhiteSpace(parts[4]))
            {
                string[] connIds = parts[4].Split(';');
                foreach (string conn in connIds)
                {
                    if (int.TryParse(conn, out int cid))
                        connections.Add(cid);
                }
            }

            MapNodeData node = new MapNodeData
            {
                nodeId = id,
                nodeType = type,
                position = new Vector2(posX, posY),
                connectedNodeIds = connections
            };

            map.nodes[id] = node;
        }

        return map;
    }
}
