using System.Collections.Generic;
using UnityEngine;

public class EnemyBoardView : MonoBehaviour
{
    // 캔버스 UI의 적 배치 슬롯 (빈 오브젝트나 패널들)
    [SerializeField] private List<Transform> slots;

    // 현재 화면에 생성된 EnemyView들을 저장하는 리스트
    public List<EnemyView> EnemyViews { get; private set; } = new();

    /// 새로운 적을 슬롯에 생성하고 EnemyViews에 등록
    // <param name= enemyData>적의 데이터 (체력, 이미지 등)</param>
    public void AddEnemy(EnemyData enemyData)
    {
        // 현재 적 수를 기준으로 빈 슬롯을 선택
        Transform slot = slots[EnemyViews.Count];
        Debug.Log($"[AddEnemy] 슬롯에 적 추가: {slot.name}");

        // EnemyView 생성: parent(슬롯)에 자식으로 생성됨
        EnemyView enemyView = EnemyViewCreator.Instance.CreateEnemyView(enemyData, slot);

        // 필요 시 위치, 스케일 조정은 프리팹 내에서 처리 권장

        // 생성된 EnemyView를 리스트에 저장
        EnemyViews.Add(enemyView);
        Debug.Log($"[AddEnemy] 현재 적 수: {EnemyViews.Count}"); // 추가
    }
}
