using System.Collections.Generic;
using UnityEngine;

public class MapNodeData
{
    public int nodeId;
    public string nodeType;
    public Vector2 position;
    // 연결된 노드를 리스트로 연결
    public List<int> connectedNodeIds = new();

    //노드를 보여주거나 선택
    public bool isCleared = false;
    public bool isSelected = false;
    public bool isVisible = false;
    
}
