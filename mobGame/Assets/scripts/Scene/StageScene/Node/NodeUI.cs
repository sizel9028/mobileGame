using UnityEngine;
using UnityEngine.UI;

public class NodeUI : MonoBehaviour
{
    [Header("UI Components")]
    public Button button;
    public Image nodeImage;

    [Header("Icon Sprties")]
    public Sprite battleIcon;
    public Sprite shopIcon;
    public Sprite campfireIcon;
    public Sprite eliteIcon;
    public Sprite treasureIcon;
    public Sprite unknownIcon;
    public Sprite clearedIcon;

    private int nodeIndex;
    //씬 매니저 호출, 정보 전달
    private UImanager manager;

    private Color defaultColor = Color.white;
    private Color disabledColor = Color.gray;
    private Color selectedColor = new Color(0.9f, 0.75f, 0.1f);
    private bool isInteractable = false;

    public void Setup(int index, NodeType type, UImanager sceneManager)
    {
        nodeIndex = index;
        manager = sceneManager;

        button.image.sprite = GetIcon(type);
        nodeImage.color = defaultColor;

        button.onClick.RemoveAllListeners();
        button.onClick.AddListener(OnClick);
    }

    private void OnClick()
    {
        //TODO manager에게 선택되었다는 정보 넘김
        manager.SetSelectedNode(nodeIndex);
    }

    public void SetSelected(bool isSelected)
    {
        nodeImage.color = isSelected ? selectedColor : defaultColor;
    }

    public void SetInteractable(bool canClick)
    {
        isInteractable = canClick;
        button.interactable = canClick;
        nodeImage.color = canClick ? defaultColor : disabledColor;
    }

    private Sprite GetIcon(NodeType type)
    {
        return type switch
        {
            NodeType.Battle => battleIcon,
            NodeType.Shop => shopIcon,
            NodeType.Campfire => campfireIcon,
            NodeType.Elite => eliteIcon,
            NodeType.Treasure => treasureIcon,
            NodeType.Unknown => unknownIcon,
            NodeType.cleared => clearedIcon,
            _ => unknownIcon
        };
    }

    //UImanager에 선택가능한지 정보 넘김
    public bool CanSelect()
    {
        return isInteractable;
    }
}
