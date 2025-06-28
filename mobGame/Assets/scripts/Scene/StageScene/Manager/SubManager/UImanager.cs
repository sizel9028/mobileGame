using System.Collections.Generic;
using Microsoft.Unity.VisualStudio.Editor;
using UnityEngine;

public class UImanager : MonoBehaviour
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

        if (selectedIdx != -1 && nodeUIs.ContainsKey(selectedIdx))
            nodeUIs[selectedIdx].SetSelected(false);

        selectedIdx = index;

        if (nodeUIs.ContainsKey(index))
            nodeUIs[index].SetSelected(true);
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
        MapNodeData latestClear = null;
        selectedIdx = -1;

        for (int i = 0; i < map.nodes.Length; ++i)
        {
            var node = map.nodes[i];
            if (node == null || node.nodeType != NodeType.cleared) continue;

            //NodeUI를 인스턴스화 시키고 등록시킴
            var ui = CreateNodeUI(node);
            RegisterNode(node.nodeId, ui);
            ui.SetInteractable(false);

            if (latestClear == null || node.nodeId > latestClear.nodeId)
                latestClear = node;
        }

        // 맵의 시작은 하나의 노드만 추가
        if (latestClear == null)
        {
            var startNode = map.nodes[0];
            if (startNode != null)
            {
                var ui = CreateNodeUI(startNode);
                RegisterNode(startNode.nodeId, ui);
                ui.SetInteractable(true);
            }
            return;
        }

        // 실제 움직일수있는 선택지 표시
        foreach (int nextId in latestClear.connectedNodeIds)
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
            if (node == null || node.nodeType != NodeType.cleared)
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

}
