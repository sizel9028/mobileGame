using UnityEngine;
using UnityEngine.UI.Extensions;

public class ArrowUIBezier : MonoBehaviour
{
    public Canvas uiCanvas;
    public UILineRenderer uiLine;    // 새로 다운받음 (spline 만듬)
    public RectTransform arrowHeadRect;    // 화살표

    [Header("커브 설정")]
    [Range(0, 1f)] public float curvature = 0.3f;
    public int segmentCount = 20;

    private bool isDragging = false;
    private Vector2 startAnchoredPos;

    void Start()
    {
        // 외부에서 Points 주입 모드 (꼭 넣어야 함)
        uiLine.drivenExternally = true;

        // 시작 시 비활성화
        uiLine.gameObject.SetActive(false);
        arrowHeadRect.gameObject.SetActive(false);
    }


    // -- 입력 받아 화살표를 보여줌 (코드 설명은 아래 전체 코드를 참고) --
    public void StartArrow(Vector2 screenStart)
    {
        isDragging = true;

        RectTransformUtility.ScreenPointToLocalPointInRectangle(
        uiCanvas.transform as RectTransform,
        screenStart, uiCanvas.worldCamera,
        out startAnchoredPos
        );

        uiLine.gameObject.SetActive(true);
        arrowHeadRect.gameObject.SetActive(true);
        uiLine.Points = new Vector2[0];
        uiLine.SetVerticesDirty();
        arrowHeadRect.anchoredPosition = startAnchoredPos;
        arrowHeadRect.localRotation = Quaternion.identity;
    }

    public void UpdateArrow(Vector2 screenEnd)
    {
        if (!isDragging) return;

        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            uiCanvas.transform as RectTransform,
            screenEnd, uiCanvas.worldCamera,
            out Vector2 endAnchoredPos
        );

        Vector2 dir = endAnchoredPos - startAnchoredPos;
        float length = dir.magnitude;
        Vector2 mid = (startAnchoredPos + endAnchoredPos) * 0.5f;
        float signY = Mathf.Sign(dir.y);
        Vector2 control = mid + Vector2.up * (length * curvature) * signY;

        Vector2[] points = new Vector2[segmentCount];
        for (int i = 0; i < segmentCount; i++)
        {
            float t = i / (float)(segmentCount - 1);
            points[i] = (1 - t) * (1 - t) * startAnchoredPos
                    + 2 * (1 - t) * t * control
                    + t * t * endAnchoredPos;
        }

        uiLine.Points = points;
        uiLine.SetVerticesDirty();

        arrowHeadRect.anchoredPosition = endAnchoredPos;
        Vector2 tailDir = points[segmentCount - 1] - points[segmentCount - 2];
        float angle = Mathf.Atan2(tailDir.y, tailDir.x) * Mathf.Rad2Deg;
        arrowHeadRect.localRotation = Quaternion.Euler(0, 0, angle);
    }

    public void EndArrow()
    {
        isDragging = false;
        uiLine.gameObject.SetActive(false);
        arrowHeadRect.gameObject.SetActive(false);
    }

    // -- Test 버전으로 제작함 -- (참고용)
    /*private void MouseTouch() 
    {
        // 드래그 시작
        if (Input.GetMouseButtonDown(0))
        {
            isDragging = true;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(
                uiCanvas.transform as RectTransform,
                Input.mousePosition, uiCanvas.worldCamera,
                out startAnchoredPos
            );   // 마우스 좌표를 캔버스 로컬 좌표로 변환

            uiLine.Points = new Vector2[0];
            uiLine.SetVerticesDirty();  // 이전 선의 값들 초기화

            arrowHeadRect.anchoredPosition = startAnchoredPos;
            arrowHeadRect.localRotation = Quaternion.identity;  // 이전 head 값 현재 위치로 바꾸기

            uiLine.gameObject.SetActive(true);
            arrowHeadRect.gameObject.SetActive(true);  // 눌렀으니 활성화
        }
        else if (isDragging && Input.GetMouseButton(0))  // 드래그 중
        {
            // 현재 마우스 로컬 좌표
            RectTransformUtility.ScreenPointToLocalPointInRectangle(
                uiCanvas.transform as RectTransform,
                Input.mousePosition, uiCanvas.worldCamera,
                out Vector2 endAnchoredPos
            );

            Vector2 dir = endAnchoredPos - startAnchoredPos;
            float length = dir.magnitude;
            Vector2 mid = (startAnchoredPos + endAnchoredPos) * 0.5f;

            //여기서 수직 방향으로만 곡률을 적용
            float signY = Mathf.Sign(dir.y);  // 위로 드래그 시 +1, 아래로 드래그 시 -1 (위로 드래그 시 위로볼록한 형태)
            Vector2 control = mid + Vector2.up * (length * curvature) * signY;

            // 샘플링
            Vector2[] points = new Vector2[segmentCount];
            for (int i = 0; i < segmentCount; i++)
            {
                float t = i / (float)(segmentCount - 1);
                points[i] = (1 - t) * (1 - t) * startAnchoredPos
                          + 2 * (1 - t) * t * control
                          + t * t * endAnchoredPos;
            }

            // Points 할당, 더티 표시
            uiLine.Points = points;
            uiLine.SetVerticesDirty();

            // 화살촉 위치/회전
            arrowHeadRect.anchoredPosition = endAnchoredPos;

            // 마지막 곡선 방향 기반 회전
            Vector2 tailDir = points[segmentCount - 1] - points[segmentCount - 2];
            float angle = Mathf.Atan2(tailDir.y, tailDir.x) * Mathf.Rad2Deg;
            arrowHeadRect.localRotation = Quaternion.Euler(0, 0, angle);
        }
        // 드래그 종료
        else if (isDragging && Input.GetMouseButtonUp(0))
        {
            isDragging = false;
            uiLine.gameObject.SetActive(false);
            arrowHeadRect.gameObject.SetActive(false);
        }
    } */
}
