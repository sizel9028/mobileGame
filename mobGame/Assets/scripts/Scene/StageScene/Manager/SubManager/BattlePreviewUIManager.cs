using UnityEngine;

public class BattlePreviewUIManager : Singleton<BattlePreviewUIManager>
{
    [SerializeField] private BattlePreviewUI previewUI;
    private CanvasGroup canvasGroup;

    protected override void Awake()
    {
        base.Awake();
        canvasGroup = previewUI.GetComponent<CanvasGroup>();
        if (canvasGroup == null)
            canvasGroup = previewUI.gameObject.AddComponent<CanvasGroup>();

        Hide();
    }

    public void ShowPreview(int nodeId)
    {
        previewUI.Setup(nodeId);

        NodeUI targetNode = UImanager.Instance.GetNodeUI(nodeId);
        if (targetNode != null)
        {
            RectTransform nodeRect = targetNode.GetComponent<RectTransform>();
            RectTransform previewRect = previewUI.GetComponent<RectTransform>();

            Vector2 nodePos = nodeRect.anchoredPosition;
            float offsetX = 400f;
            float directionX = nodePos.x < -100f ? 1f : -1f;

            float x = nodePos.x + directionX * offsetX;
            float y = Mathf.Clamp(nodePos.y, -200f, 200f);
        
            previewRect.anchoredPosition = new Vector2(x, y);
        }

        canvasGroup.alpha = 1f;
        canvasGroup.interactable = true;
        canvasGroup.blocksRaycasts = true;
    }
    public void Hide()
    {
        canvasGroup.alpha = 0f;
        canvasGroup.interactable = false;
        canvasGroup.blocksRaycasts = false;
    }
}
