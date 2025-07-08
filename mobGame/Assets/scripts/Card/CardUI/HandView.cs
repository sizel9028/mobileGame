using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Splines;

public class HandView : MonoBehaviour
{
    //카드를 저장 + 배치
    [SerializeField] private SplineContainer splineContainer;
    private List<CardUI> cards = new();

    //선택 카드 + 화살표 표현하기 위함
    private CardUI selectCard = null;
    private bool isDragging = false;
    [SerializeField] private ArrowUIBezier arrowUI;

    //꾹 누르는거 표현
    private Vector2 startPos;   // 누른 시작위치 저장
    private int originalSiblingIndex;
    private Quaternion originalRotation;
    private Vector3 originalCardPos;
    private Vector3 originalCardScale;
    private float longPressTime = 1f;
    private Coroutine longPressRoutine;

    public IEnumerator AddCard(CardUI card)
    {
        cards.Add(card);
        yield return UpdateCardPositions(0.15f);
    }

    private IEnumerator UpdateCardPositions(float duration)
    {
        if (cards.Count == 0) yield break;

        float cardSpacing = 1f / 10f;
        float firstCardPosition = 0.5f - (cards.Count - 1) * cardSpacing / 2;
        Spline spline = splineContainer.Spline;
        for (int i = 0; i < cards.Count; ++i)
        {
            float p = firstCardPosition + i * cardSpacing;
            Vector3 splinePos = spline.EvaluatePosition(p);
            Vector3 forward = spline.EvaluateTangent(p);
            Vector3 up = spline.EvaluateUpVector(p);

            Quaternion rotation = Quaternion.LookRotation(-up, Vector3.Cross(-up, forward).normalized);
            cards[i].transform.DOMove(splinePos + transform.position, duration);
            cards[i].transform.DORotate(rotation.eulerAngles, duration);
        }
        yield return new WaitForSeconds(duration);
    }

    //카드 클릭 처리

    public void OnCardDown(CardUI card)
    {
        if (isDragging) return;

        selectCard = card;
        isDragging = true;
        card.SetSelected(true);

        startPos = Input.mousePosition;
        originalCardPos = card.transform.position;
        originalCardScale = card.transform.localScale;
        originalRotation = card.transform.rotation;
        originalSiblingIndex = card.transform.GetSiblingIndex();

        longPressRoutine = StartCoroutine(LongPressRoutine(card));

        // 카드의 월드 위치 기준 위쪽 중앙 계산 (한마디로 화살표 생성위치를 조금 올려서 UI적으로 어떤 카드인지 확실하게 함)
        RectTransform cardRect = card.GetComponent<RectTransform>();
        Vector3 topMiddle = card.transform.position + card.transform.up * (cardRect.rect.height * 0.5f * card.transform.lossyScale.y);

        // 화살표 시작 위치로 사용
        arrowUI.StartArrow(topMiddle);

    }

    public void OnCardUp(CardUI card)
    {
        if (!isDragging) return;
        isDragging = false;

        if (longPressRoutine != null)
        {
            StopCoroutine(longPressRoutine);
        }

        ResetZoomCard();

        selectCard = null;
        card.SetSelected(false);
        arrowUI.EndArrow();
    }

    private IEnumerator LongPressRoutine(CardUI card)
    {
        float timer = 0f;
        while (timer < longPressTime)
        {
            float distance = Vector2.Distance(Input.mousePosition, startPos);
            if (distance > 40f) yield break;

            timer += Time.deltaTime;
            yield return null;    //다음프레임까지 기다림 (안하면 초당)
        }

        ZoomCard(card);
    }

    //카드를 줌시켜서 확대한다
    private void ZoomCard(CardUI card)
    {
        arrowUI.EndArrow();   //카드 자세히보기 했을땐 필요가 없음

        RectTransform rect = card.GetComponent<RectTransform>();
        rect.SetAsLastSibling();

        Vector2 targetAnchorPos = new Vector2(-400f, 0f);

        rect.DOAnchorPos(targetAnchorPos, 0.25f).SetUpdate(true);
        rect.DOScale(originalCardScale * 1.8f, 0.25f).SetUpdate(true);
        rect.DORotate(Vector3.zero, 0.25f).SetUpdate(true);
    }

    //카드 줌을 원상태 복귀
    private void ResetZoomCard()
    {
        if (selectCard == null) return;

        RectTransform rect = selectCard.GetComponent<RectTransform>();

        rect.SetSiblingIndex(originalSiblingIndex);  // 원래 위치로
        rect.DOMove(originalCardPos, 0.25f).SetUpdate(true);
        rect.DOScale(originalCardScale, 0.25f).SetUpdate(true);
        rect.DORotate(originalRotation.eulerAngles, 0.25f).SetUpdate(true); // 회전 복구
    }

    void Update()
    {
        if (isDragging)
        {
            Vector2 mousePos = Input.mousePosition;
            arrowUI.UpdateArrow(mousePos);
        }
    }

    public void DiscardAllCards()
    {
        float delayOffset = 0.05f; //삭제되는데 걸리는 딜레이

        //선택 카드 초기화 시키기
        selectCard = null;
        isDragging = false;
        arrowUI.EndArrow();

        for (int i = 0; i < cards.Count; ++i)
        {
            CardUI card = cards[i];

            RectTransform rect = card.GetComponent<RectTransform>();
            Vector2 targetAnchorPos = new Vector2(800f, -300f);
            float duration = 0.4f;

            //움직임
            rect.DOAnchorPos(targetAnchorPos, duration).SetDelay(i * delayOffset);
            //크기 축소
            rect.DOScale(Vector3.zero, duration)
            .SetEase(Ease.InBack)
            .SetDelay(i * delayOffset);
            //회전 복구
            rect.DORotate(Vector3.zero, duration).SetDelay(i * delayOffset);

            //카드 객체 삭제
            DOVirtual.DelayedCall(i * delayOffset + duration, () => { Destroy(card.gameObject);});
        }

        cards.Clear();

    }
}
