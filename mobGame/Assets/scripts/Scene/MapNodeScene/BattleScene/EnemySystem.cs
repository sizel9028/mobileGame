using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

/// <summary>
/// 적 시스템 관리 클래스 (적 UI 생성 및 턴 처리)
/// Slay the Spire로 치면 적들이 생성되고, 턴마다 행동하는 관리 매니저 역할
/// </summary>
public class EnemySystem : MonoBehaviour
{
    // 적들이 배치될 UI 슬롯들을 관리하는 뷰
    [SerializeField] private EnemyBoardView enemyBoardView;

    [SerializeField] private List<EnemyData> testEnemies; // 인스펙터에 적 데이터 넣기
    [SerializeField] private PlayerController player;



    void Start()
    {
        Setup(testEnemies); // 전투 시작 시 자동으로 적 생성
    }

    // // 이 컴포넌트가 활성화될 때 EnemyTurn 수행자를 액션 시스템에 등록
    // void OnEnable()
    // {
    //     ActionSystem.AttachPerformer<EnemyTurn>(EnemyTurnPerform);
    // }

    // // 이 컴포넌트가 비활성화될 때 EnemyTurn 수행자 등록 해제
    // void OnDisable()
    // {
    //     ActionSystem.DetachPerformer<EnemyTurn>();
    // }

    // 위 코드들은 액션시스템 기능이라 빼야할듯?

    /// <summary>
    /// 전투 시작 시 적들의 데이터를 받아 EnemyView를 생성하여 배치
    /// </summary>
    /// <param name="enemyDatas">생성할 적들의 정보 리스트</param>
    public void Setup(List<EnemyData> enemyDatas)
    {
        foreach (var enemyData in enemyDatas)
        {
            // 적 데이터를 기반으로 EnemyView UI 생성
            Debug.Log($"[Setup] 적 생성 시도: {enemyData.health}");
            enemyBoardView.AddEnemy(enemyData);
        }
    }

    // EnemyTurn 수행 코루틴 (적 전체 턴 실행)
    // 적들이 한 명씩 순서대로 행동하는 과정
    private IEnumerator EnemyTurnCoroutine()
    {
        foreach (var enemy in enemyBoardView.EnemyViews)
        {
            yield return StartCoroutine(EnemyAttackCoroutine(enemy));
        }

        Debug.Log("End Enemy Turn");
    }

    // 적 하나가 공격하는 코루틴
    private IEnumerator EnemyAttackCoroutine(EnemyView enemy)
    {
        RectTransform rect = enemy.GetComponent<RectTransform>();

        // 현재 anchoredPosition에서 왼쪽으로 이동
        Vector2 originalPos = rect.anchoredPosition;
        Vector2 targetPos = originalPos + Vector2.left * 50f;

        Tween moveOut = rect.DOAnchorPos(targetPos, 0.15f);
        yield return moveOut.WaitForCompletion();


        // 다시 원래 자리로 복귀
        Tween moveBack = rect.DOAnchorPos(originalPos, 0.25f);
        yield return moveBack.WaitForCompletion();

        // 플레이어에게 데미지 줌
        Debug.Log("플레이어 데미지 연출 실행됨");
        player.Damage();
    }


    // 턴 시작 시 외부에서 이 함수 호출하면 적 전체 턴 실행
    // End Turn을 누르면 발생하는 순간을 처리 (적 턴 시작)
    public void StartEnemyTurn()
    {
        StartCoroutine(EnemyTurnCoroutine());
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            Debug.Log("스페이스 눌림 - Enemy 공격 모션 실행");
            StartEnemyTurn();
        }

        if (Input.GetKeyDown(KeyCode.W))
        {


            if (enemyBoardView.EnemyViews.Count > 0)
            {
                Debug.Log("W 키 눌림 - Enemy 데스 모션 실행");
                EnemyView enemy = enemyBoardView.EnemyViews[0];
                StartCoroutine(KillEnemy(enemy));
            }
        }
    }

private IEnumerator KillEnemy(EnemyView enemy)
    {
        enemyBoardView.EnemyViews.Remove(enemy);
        yield return StartCoroutine(PlayDeathAnimation(enemy.gameObject));
    }

    public IEnumerator PlayDeathAnimation(GameObject enemyObject)
    {
    EnemyView enemyView = enemyObject.GetComponent<EnemyView>();
    Sequence deathSequence = DOTween.Sequence();

    if (enemyView != null)
    {
        // 캐릭터 이미지 스케일 및 알파 제거
        if (enemyView.characterImage != null)
        {
            deathSequence.Join(enemyView.characterImage.rectTransform.DOScale(Vector3.zero, 0.3f));
            deathSequence.Join(enemyView.characterImage.DOFade(0f, 0.3f));
        }

        // 테두리 이미지도 동일하게 제거
        if (enemyView.characterBoldImage != null)
        {
            deathSequence.Join(enemyView.characterBoldImage.rectTransform.DOScale(Vector3.zero, 0.3f));
            deathSequence.Join(enemyView.characterBoldImage.DOFade(0f, 0.3f));
        }
    }

    // 전체 오브젝트도 축소
    deathSequence.Join(enemyObject.transform.DOScale(Vector3.zero, 0.3f));

    yield return deathSequence.WaitForCompletion();

    Destroy(enemyObject);
    }



}

