using System.Collections.Generic;
using UnityEngine;

public class UImanager : Singleton<UImanager>
{
    //노드 프리팹
    public GameObject nodePrefab;
    public RectTransform nodeParent;

    //노드를 이어줌
    public GameObject linePrefab;
    public RectTransform lineParent;

    private Dictionary<int, NodeUI> nodeUIs = new();
    private int selectedIdx = -1;
    public int SelectedIdx => selectedIdx;

    //manager가 먼저 버튼을 전부 등록하고, 그 등록한 버튼의 활성화 여부까지 등록해야함

    //버튼 등록하기
    public void RegisterNode(int index, NodeUI node)
    {
        nodeUIs[index] = node;
    }

    //버튼 바꾸기, 만약 활성화 되어 있지 않다면 바꾸지 않음
    public void SetSelectedNode(int index)
    {
        if (!nodeUIs.ContainsKey(index))
            return;

        if (!nodeUIs[index].CanSelect())
            return;

        if (selectedIdx == index)
        {
            nodeUIs[index].SetSelected(false);
            selectedIdx = -1;
            BattlePreviewUIManager.Instance.Hide();

            return;
        }

        if (selectedIdx != -1 && nodeUIs.ContainsKey(selectedIdx))
            nodeUIs[selectedIdx].SetSelected(false);

        selectedIdx = index;

        if (nodeUIs.ContainsKey(index))
            nodeUIs[index].SetSelected(true);

        NodeType type = nodeUIs[index].GetNodeType();

        if (type == NodeType.Battle || type == NodeType.Elite || type == NodeType.Boss || type == NodeType.Treasure)
        {
            BattlePreviewUIManager.Instance.ShowPreview(index);
        }
        else
        {
            BattlePreviewUIManager.Instance.Hide();
        }

    }

    //버튼 활성화 또는 잠금 시키기
    /*public void SetNodeInteractable(int index, bool canClick)
    {
        if (nodeUIs.ContainsKey(index))
            nodeUIs[index].SetInteractable(canClick);
    }*/

    public void InitMap(MapNode map)
    {
        nodeUIs.Clear();
        selectedIdx = -1;

        var startNode = map.nodes[0]; // 시작 노드 설정
        if (startNode == null) return;

        if (startNode.nodeType != NodeType.Cleared)
        {
            var ui = CreateNodeUI(startNode);
            RegisterNode(startNode.nodeId, ui);
            ui.SetInteractable(true);
            return;
        }

        MapNodeData lastCleared = FindLastClearedNode(map, startNode);

        foreach (var node in map.nodes)
        {
            if (node == null || node.nodeType != NodeType.Cleared) continue;
            var ui = CreateNodeUI(node);
            RegisterNode(node.nodeId, ui);
            ui.SetInteractable(false);
        }

        foreach (int nextId in lastCleared.connectedNodeIds)
        {
            var node = map.nodes[nextId];
            if (node == null) continue;
            if (nodeUIs.ContainsKey(node.nodeId)) continue;

            var ui = CreateNodeUI(node);
            RegisterNode(node.nodeId, ui);
            ui.SetInteractable(true);
        }

        DrawAllLine(map);
    }

    //DFS 탐색 :: 가장 마지막에 깬 클리어 노드를 리턴
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
                return current;
            }

            current = nextCleared;
        }
    }

    private NodeUI CreateNodeUI(MapNodeData node)
    {
        // TODO 게임오브젝트 인스턴스화 후 NodeUI 반환
        //return null;

        GameObject obj = Instantiate(nodePrefab, nodeParent);
        NodeUI ui = obj.GetComponent<NodeUI>();

        RectTransform rt = obj.GetComponent<RectTransform>();
        rt.anchoredPosition = node.position;

        ui.Setup(node.nodeId, node.nodeType, this);
        return ui;
    }

    private void DrawAllLine(MapNode map)
    {
        foreach (var node in map.nodes)
        {
            if (node == null || node.nodeType != NodeType.Cleared)
                continue;

            if (!nodeUIs.ContainsKey(node.nodeId))
                continue;

            NodeUI fromUI = nodeUIs[node.nodeId];

            foreach (int toId in node.connectedNodeIds)
            {
                if (!nodeUIs.ContainsKey(toId))
                    continue;

                NodeUI toUI = nodeUIs[toId];
                DrawLine(fromUI, toUI);
            }
        }
    }

    private void DrawLine(NodeUI from, NodeUI to)
    {
        GameObject lineObj = Instantiate(linePrefab, lineParent);
        RectTransform rt = lineObj.GetComponent<RectTransform>();

        Vector2 start = from.GetComponent<RectTransform>().anchoredPosition;
        Vector2 end = to.GetComponent<RectTransform>().anchoredPosition;
        Vector2 dir = end - start;

        float length = dir.magnitude;
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;

        rt.sizeDelta = new Vector2(length, 4);
        rt.anchoredPosition = start + dir * 0.5f;
        rt.localRotation = Quaternion.Euler(0, 0, angle);
    }

    public NodeType? GetNodeType()
    {
        if (selectedIdx == -1 || !nodeUIs.ContainsKey(selectedIdx))
            return null;

        return nodeUIs[selectedIdx].GetNodeType();
    }

    public NodeUI GetNodeUI(int nodeId)
    {
        if (nodeUIs.TryGetValue(nodeId, out var ui))
            return ui;
        return null;
    }

}
