using System;
using System.Collections.Generic;
using UnityEngine;

public class MapGenerator : MonoBehaviour
{
    // csv파일로 저장되어 있는 맵을 불러옴

    private static readonly Dictionary<NodeType, float> nodeProbabilities = new()
    {
        { NodeType.Battle, 0.45f },
        { NodeType.Campfire, 0.12f },
        { NodeType.Elite, 0.15f },
        { NodeType.Shop, 0.15f },
        //{ NodeType.Unknown, 0.6f },
        { NodeType.Treasure, 0.13f },
    };

    public static MapNode LoadMap(int stage, int themeNumber, int mapKind = -1)
    {
        if (mapKind < 0)
        {
            //TODo 각 스테이지별로 맵의 갯수를 선언
            int maxKind = 3;
            mapKind = UnityEngine.Random.Range(1, maxKind+1);
        }

        //string path = $"Maps/Stage{stage}_{themeNumber}_{mapKind}";
        string path = $"Maps/{mapKind}";
        //string path = $"Maps/4";    //맵 잘 만들어졌는지 확인용
        TextAsset csvFile = Resources.Load<TextAsset>(path);

        if (csvFile == null)
        {
            return null;
        }

        MapNode map = new MapNode();
        map.stageNumber = stage;

        if (stage == 0)
        {
            map.theme = MapTheme.NOP;
        }
        else
        {
            //TODO 맵별 기믹이 잘 작동하는지 체크용도
            //map.theme = MapTheme.VOID;
            map.theme = GameManager.gameManager.playerData.currentMap.theme;
        }

        map.nodes = new List<MapNodeData>();
        int lastNodeId = -1;

        string[] lines = csvFile.text.Split('\n');
        for (int i = 1; i < lines.Length; ++i)
        {
            string line = lines[i].Trim();
            if (string.IsNullOrWhiteSpace(line))
                continue;

            string[] parts = line.Split(',');

            int id = int.Parse(parts[0]);
            lastNodeId = Mathf.Max(lastNodeId, id);
            //NodeType type = Enum.Parse<NodeType>(parts[1]);
            NodeType type;
            if (stage == 0)
            {
                type = Enum.Parse<NodeType>(parts[1]);
            }
            else
            {
                type = GetRandomNodeType();
            }
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

            map.nodes.Add(node);
        }

        //마지막 노드 보스로 바꿈
        if (lastNodeId >= 0 && map.nodes[lastNodeId] != null)
        {
            map.nodes[lastNodeId].nodeType = NodeType.Boss;
        }

        return map;
    }

    private static NodeType GetRandomNodeType()
    {
        float rand = UnityEngine.Random.value;
        float cumulative = 0f;

        foreach (var pair in nodeProbabilities)
        {
            cumulative += pair.Value;
            if (rand <= cumulative)
                return pair.Key;
        }

        // 만약 float 오차로 인해 못 뽑는 경우 fallback
        return NodeType.Unknown;
    }

}
