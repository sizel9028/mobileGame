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
    private bool isZooming = false;
    [SerializeField] private ArrowUIBezier arrowUI;

    //꾹 누르는거 표현
    private Vector2 startPos;   // 누른 시작위치 저장
    private int originalSiblingIndex;
    private Quaternion originalRotation;
    private Vector3 originalCardPos;
    private Vector3 originalCardScale;
    private float longPressTime = 1f;
    private Coroutine longPressRoutine;

    private CardProcessor processor = new();  // 카드 실행 클래스

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

        Debug.Log($"[HandView] 카드 다운: {card.data.nameKey}, 코스트: {card.data.cost}, 사용 가능 여부: {CardValidator.IsCardAble(card.data, true)}");


        if (isDragging) return;
        if (!CardValidator.IsCardAble(card.data, true)) return;

        selectCard = card;
        isDragging = true;
        card.SetSelected(true);

        startPos = Input.mousePosition;
        originalCardPos = card.transform.position;
        originalCardScale = card.transform.localScale;
        originalRotation = card.transform.rotation;
        originalSiblingIndex = card.transform.GetSiblingIndex();

        longPressRoutine = StartCoroutine(LongPressRoutine(card));

        arrowUI.SetValidator(card); //누르면 값을 저장

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

        card.SetSelected(false);
        var ui = arrowUI.EndArrow();

        bool isCancel = arrowUI.GetInCancleZone(); //취소 카드인지 확인

        if (!isZooming && !isCancel)
        {
            ProcessCard(ui);
        }

        selectCard = null;  //카드 널로 바꿈
        CheckUsableCard();
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

    //카드 효과 적용 함수 (ui는 enemyUI임)
    private void ProcessCard(CharacterUI ui)
    {
        var card = selectCard?.data;
        if (card == null)
        {
            Debug.LogWarning("[ProcessCard] selectCard가 null임");
            return;
        }

        if ((card.cardTarget == CardTarget.oneEnemy || card.cardTarget == CardTarget.onePlayer) && ui == null)
        {
            Debug.LogWarning("[ProcessCard] 단일 타겟 카드인데 타겟이 null이라 실행 안 함");
            return;
        }
        
        Debug.Log("processCard 실행");
        //Debug.Log($"[ProcessCard] 실행, selectCard: {(selectCard != null ? selectCard.data.nameKey : "null")}, targetUI: {(ui != null ? ui.name : "null")}");
        processor.SpendCost(card, ui, this);
        var characterUI = CharacterUIManager.Instance.playerUIs[0]; // 플레이어 UI를 받아서 caster로 넘김
        processor.ProcessCardWithTarget(card, characterUI, ui);

        //TODO 카드 실행후 삭제 로직 추가
    }

    

    //카드를 줌시켜서 확대한다
    private void ZoomCard(CardUI card)
    {
        isZooming = true;  //줌 활성화

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

        isZooming = false;

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
            DOVirtual.DelayedCall(i * delayOffset + duration, () => { Destroy(card.gameObject); });
        }

        cards.Clear();

    }

    public void CheckUsableCard()  //불가능한 카드는 색을 넣지 않음
    {
        foreach (var card in cards)
        {
            card.UpdateUsableVisual();
        }
    }
}
