using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
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

    private CharacterUI currentTarget = null;
    private CancelZoneMarker lastMarker = null; 
    private TargetingValidator targetingValidator = new();

    //캔슬존 판단 확인 변수
    private bool isInCancelZone = false;

    // --- test --- (디버그 용도)
    public TextMeshProUGUI debugText;

    void Start()
    {
        // 외부에서 Points 주입 모드 (꼭 넣어야 함)
        uiLine.drivenExternally = true;

        // 시작 시 비활성화
        uiLine.gameObject.SetActive(false);
        arrowHeadRect.gameObject.SetActive(false);
    }

    public void SetValidator(CardUI cardUI)  //현재 선택된 카드의 정보를 지니고 있음
    {
        targetingValidator.SetSelectCard(cardUI);
    }


    // -- 입력 받아 화살표를 보여줌  --
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

    private void CheckTargetUnderArrow()
    {
        /*Vector2 worldPos = arrowHeadRect.position;

        PointerEventData eventData = new PointerEventData(EventSystem.current);
        eventData.position = worldPos;  // 해당 점을 기준으로 충돌 객체 조사 */

        Vector2 screenPos = RectTransformUtility.WorldToScreenPoint(uiCanvas.worldCamera, arrowHeadRect.position);

        PointerEventData eventData = new PointerEventData(EventSystem.current);
        eventData.position = screenPos;

        List<RaycastResult> results = new();
        EventSystem.current.RaycastAll(eventData, results); // results에 저장

        CharacterUI foundTarget = null;
        isInCancelZone = false;
        CancelZoneMarker foundMarker = null;

        foreach (var result in results)
        {
            CharacterUI chUI = result.gameObject.GetComponentInParent<CharacterUI>();  //부모 오브젝트 찾아야함 실제 닿는건 image(자식오브젝트)

            if (chUI != null && targetingValidator != null && targetingValidator.isTargetAble(chUI))
            {
                foundTarget = chUI;
                break;
            }

            var Marker = result.gameObject.GetComponentInParent<CancelZoneMarker>();

            if (Marker != null)  // 캔슬존 확인
            {
                isInCancelZone = true;
                foundMarker = Marker;
            }
        }

        /*if (debugText != null)
        {
            if (foundTarget != null && foundTarget.character != null)
            {
                debugText.text = $"Found: {foundTarget.character.characterArtName}";
            }
            else
            {
                debugText.text = "Found: null";
            }
        }*/

        if (currentTarget != foundTarget)
        {
            if (currentTarget != null) currentTarget.SetOutlineColor(false);
            if (foundTarget != null) foundTarget.SetOutlineColor(true);

            currentTarget = foundTarget;
        }

        if (lastMarker != foundMarker)
        {
            if (lastMarker != null) lastMarker.SetColor(false);
            if (foundMarker != null) foundMarker.SetColor(true);

            lastMarker = foundMarker;   
        }
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

        CheckTargetUnderArrow();   // chUI 체크
    }

    public CharacterUI EndArrow()
    {
        isDragging = false;

        var returnDummy = currentTarget;

        if (currentTarget != null)
        {
            currentTarget.SetOutlineColor(false);
            currentTarget = null;
        }

        if (lastMarker != null)
        {
            lastMarker.SetColor(false);
            lastMarker = null;
        }

        uiLine.gameObject.SetActive(false);
        arrowHeadRect.gameObject.SetActive(false);

        return returnDummy;
    }

    public bool GetInCancleZone()
    {
        return isInCancelZone;
    }

}
