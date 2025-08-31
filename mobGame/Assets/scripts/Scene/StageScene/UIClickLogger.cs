using UnityEngine;

public class UIClickLogger : MonoBehaviour
{
    [SerializeField] private RectTransform canvasRect; // Canvas의 RectTransform

    void Update()
    {
        if (Input.GetMouseButtonDown(0)) // 좌클릭
        {
            Vector2 localPoint;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(
                canvasRect, 
                Input.mousePosition, 
                null, // 카메라 (Screen Space - Overlay 캔버스는 null)
                out localPoint
            );

            Debug.Log($"UI Local Position: {localPoint}");
        }
    }
}
