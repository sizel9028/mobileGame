using System.Collections.Generic;
using UnityEngine;

public enum NodeType
{
    Battle, Shop, Campfire, Elite, Treasure, Unknown, Cleared
}

public class MapNodeData
{
    public int nodeId;
    public NodeType nodeType;
    public Vector2 position;
    // 연결된 노드를 리스트로 연결
    public List<int> connectedNodeIds = new();

    //노드를 보여주거나 선택
    //public bool isCleared = false;
    //public bool isSelected = false; NodeUI가 관리
    //public bool isVisible = false;

}
