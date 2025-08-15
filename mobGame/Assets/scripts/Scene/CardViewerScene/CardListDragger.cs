using UnityEngine;
using UnityEngine.EventSystems;

public class CardListDragger : MonoBehaviour, IBeginDragHandler, IDragHandler
{
    [SerializeField] private RectTransform target;
    [SerializeField] private float dragSpeed = 1f;

    private float maxY = 0f;
    private float minY = 0f;
    private Vector2 prevPos;

    void Start()
    {
        int count = GameManager.gameManager.CardViewCards.cards.Count;
        if (count < 11) maxY = 0f;
        else
        {
            int rows = Mathf.CeilToInt(count / 5f);
            int scrollableRows = Mathf.Max(0, rows - 2);
            maxY = scrollableRows * 440f;
        }   
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        prevPos = eventData.position;
    }

    public void OnDrag(PointerEventData eventData)
    {
        Vector2 delta = eventData.position - prevPos;
        prevPos = eventData.position;

        float newY = target.anchoredPosition.y + delta.y * dragSpeed;
        newY = Mathf.Clamp(newY, minY, maxY);

        target.anchoredPosition = new Vector2(target.anchoredPosition.x, newY);
    }
}