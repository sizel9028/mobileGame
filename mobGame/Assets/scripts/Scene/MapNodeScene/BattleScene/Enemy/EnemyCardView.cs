using System.Collections;
using DG.Tweening;
using UnityEngine;

//에너미가 어떤 카드 쓰는지 보여줌
public class EnemyCardView : Singleton<EnemyCardView>
{
    [SerializeField] private CardUIManager cardUIManager;  // 카드 만들때 사용
    [SerializeField] private Transform enemyCardParent;  // 어디에 모일건지

    private Vector2 spawnPos = new Vector2(0, 800f);  // 시작 위치 (밑에서 올라옴)
    private Vector2 showPos = new Vector2(0, 0);       // 보여줄 위치 (중앙)
    private Vector2 exitPos = new Vector2(0, 800f);    // 퇴장 위치 (위로 사라짐)

    public IEnumerator ShowEnemyCardAndWait(CardData cardData)
    {
        // 1. 카드 생성
        CardUI cardUI = cardUIManager.CreateCard(cardData, enemyCardParent, spawnPos);
        RectTransform rect = cardUI.GetComponent<RectTransform>();

        cardUI.SetManager(null);
        cardUI.SetHandView(null);

        // 2. 0.1초 렌더링 준비 시간
        yield return new WaitForSeconds(0.1f);

        // 3. 카드 올라오는 연출
        yield return rect.DOAnchorPos(showPos, 0.3f).SetEase(Ease.OutBack).WaitForCompletion();

        // 4. 카드 위치 도달 시 효과 실행
        //yield return Battle.Instance.StartCoroutine(EnemySystem.Instance.PlayCard(cardData));

        // 5. 1초 동안 카드 보여줌
        yield return new WaitForSeconds(0.7f);

        // 6. 위로 사라지는 연출
        yield return rect.DOAnchorPos(exitPos, 0.4f).SetEase(Ease.InBack).WaitForCompletion();

        // 7. 카드 오브젝트 제거
        cardUI.DestroyCard();

        yield return Battle.Instance.StartCoroutine(EnemySystem.Instance.PlayCard(cardData));

        // 8. 템포 조절용 여유 시간
        yield return new WaitForSeconds(0.5f);
    }

}
