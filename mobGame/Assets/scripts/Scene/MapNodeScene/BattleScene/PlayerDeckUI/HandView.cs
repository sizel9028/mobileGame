using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Splines;

public class HandView : MonoBehaviour
{
    //카드를 저장 + 배치
    [SerializeField] private SplineContainer splineContainer;
    public List<CardUI> cards = new();

    //선택 카드 + 화살표 표현하기 위함
    private CardUI selectCard = null;
    private bool isDragging = false;
    private bool isZooming = false; //지금 줌을 하기 위해 꾹 누르고 있는지 체크
    private bool isZoom = false; //실제 줌 중인지

    private bool isResettingZoom = false;
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
        //카드 널일때도 추가

        float cardSpacing = 1f / 10f;
        float firstCardPosition = 0.5f - (cards.Count - 1) * cardSpacing / 2;
        Spline spline = splineContainer.Spline;

        for (int i = 0; i < cards.Count; ++i)
        {
            //카드 널일때는 넘겨서 Tween null 방지
            if (cards[i] == null || cards[i].gameObject == null || cards[i].transform == null) continue;

            float p = firstCardPosition + i * cardSpacing;
            Vector3 splinePos = spline.EvaluatePosition(p);
            Vector3 forward = spline.EvaluateTangent(p);
            Vector3 up = spline.EvaluateUpVector(p);

            Quaternion rotation = Quaternion.LookRotation(-up, Vector3.Cross(-up, forward).normalized);

            RectTransform rect = cards[i].GetComponent<RectTransform>();
            Vector2 targetPos = new Vector2(splinePos.x, splinePos.y - 500f);
            rect.DOAnchorPos(targetPos, duration);
            rect.DORotate(rotation.eulerAngles, duration);
        }
        yield return new WaitForSeconds(duration);
    }

    //카드 클릭 처리

    public void OnCardDown(CardUI card)
    {

        Debug.Log($"[HandView] 카드 다운: {card.data.nameKey}, 코스트: {card.data.cost}, 사용 가능 여부: {CardValidator.IsCardAble(card.data, true)}");


        if (isDragging || isZoom || isResettingZoom) return;
        if (!CardValidator.IsCardAble(card.data, true)) return;

        selectCard = card;
        isDragging = true;
        isZooming = true;
        card.SetSelected(true);

        //startPos = Input.mousePosition; (pc 버전)
        startPos = GetInputPosition();
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

        card.SetSelected(false);
        var ui = arrowUI.EndArrow();

        bool isCancel = arrowUI.GetInCancleZone(); //취소 카드인지 확인
        bool isProcessingCard = Battle.Instance.isProcessingCard;  //카드 작동중이면 다음카드 실행을 불가능하게 함

        bool canUseCard = !isZooming && !isCancel && !isZoom && !isProcessingCard;

        if (canUseCard)   // 실행 가능한 상태인지
        {
            ProcessCard(ui);
        }

        ResetZoomCard();
        selectCard = null;  //카드 널로 바꿈
        CheckUsableCard();
    }

    private IEnumerator LongPressRoutine(CardUI card)
    {
        float timer = 0f;
        while (timer < longPressTime)
        {
            //float distance = Vector2.Distance(Input.mousePosition, startPos); pc 버전
            float distance = Vector2.Distance(Input.mousePosition, startPos);

            if (distance > 40f)
            {
                isZooming = false;
                yield break;
            }

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
        StartCoroutine(processor.ProcessCardWithTarget(card, characterUI, ui));

        //TODO 카드 실행후 삭제 로직 추가
        RemoveCard(selectCard);
    }

    public void RemoveCard(CardUI card)
    {
        if (!cards.Contains(card)) return;

        //스크롤 카드라면 스크롤에서 제거
        if (card.data.cardType == CardType.Scroll)
        {
            DeckManager.Instance.scrollCards.Remove(card.data);
        }
        else if (card.data.rare != Rare.TierRage)
        {
            DeckManager.Instance.discardPile.cards.Add(card.data);
        }

        //일반 카드라면
        // 1) 덱에 폐기 등록

        cards.Remove(card);

        // 2) RectTransform 가져오기
        RectTransform rect = card.GetComponent<RectTransform>();
        float duration = 0.25f;

        // 3) 기존 트윈 정리
        rect.DOKill();

        // 4) 시퀀스 구성
        var seq = DOTween.Sequence()
            .SetLink(card.gameObject)                // 카드가 파괴되면 자동으로 트윈도 죽임
            .Append(rect.DOScale(Vector3.zero, duration).SetEase(Ease.InBack))  // 작아지면서 사라짐
            .Join(rect.DORotate(Vector3.zero, duration))            // 회전 초기화
            .OnComplete(() =>
            {
                card.DestroyCard();                  // 내부에서 transform.DOKill(true) + Destroy(gameObject)
                StartCoroutine(UpdateCardPositions(0.2f));  //남은 카드 재정렬
            });
    }


    //카드를 줌시켜서 확대한다
    private void ZoomCard(CardUI card)
    {
        isZooming = false;  //줌 활성화
        isZoom = true;

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
        isZoom = false;
        isResettingZoom = true;

        RectTransform rect = selectCard.GetComponent<RectTransform>();

        rect.SetSiblingIndex(originalSiblingIndex);  // 원래 위치로
        rect.DOMove(originalCardPos, 0.25f).SetUpdate(true);
        rect.DOScale(originalCardScale, 0.25f).SetUpdate(true);
        rect.DORotate(originalRotation.eulerAngles, 0.25f).SetUpdate(true)
            .OnComplete(() => { isResettingZoom = false; }); // 회전 복구

    }

    void Update()
    {
        if (isDragging)
        {
            //Vector2 mousePos = Input.mousePosition; (pc)

            Vector2 inputPos = GetInputPosition();
            arrowUI.UpdateArrow(inputPos);
        }
    }

    public void DiscardAllCards()
    {
        float delayOffset = 0.05f; //삭제되는데 걸리는 딜레이

        //선택 카드 초기화 시키기
        selectCard = null;
        isDragging = false;
        isZoom = false;
        isZooming = false;
        arrowUI.EndArrow();

        for (int i = 0; i < cards.Count; ++i)
        {
            CardUI card = cards[i];
            if (card == null || card.gameObject == null) continue;

            if (card.data.rare != Rare.TierRage && card.data.cardType != CardType.Scroll)
            {
                DeckManager.Instance.discardPile.cards.Add(card.data);
            }

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
            rect.DORotate(Vector3.zero, duration).SetDelay(i * delayOffset).OnComplete(() => { card.DestroyCard(); });
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

    //PC 테스트용 모바일 버전 용도 함수
    private Vector2 GetInputPosition()
    {
        if (Input.touchCount > 0)
        {
            return Input.GetTouch(0).position;
        }
        else
        {
            return Input.mousePosition; // 에디터/PC 테스트용
        }
    }
}
