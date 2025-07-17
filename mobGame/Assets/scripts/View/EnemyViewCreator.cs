using UnityEngine;

public class EnemyViewCreator : Singleton<EnemyViewCreator>
{
    // 프리팹으로 등록된 EnemyView (Canvas에 들어갈 프리팹)
    [SerializeField] private EnemyView enemyViewPrefab;

    // 적 UI를 생성하는 함수
    // enemyData: 적 데이터
    // parent: 생성될 부모 오브젝트 (예: EnemyBoardView의 Slot)
    public EnemyView CreateEnemyView(EnemyData enemyData, Transform parent)
    {
        // 프리팹을 캔버스 내에 자식으로 생성 (로컬 위치/회전 유지)
        EnemyView enemyView = Instantiate(enemyViewPrefab, parent);

        // EnemyView 초기화 (Setup은 enemyData를 받아서 체력/이미지 등을 설정하는 함수여야 함)
        enemyView.Setup(enemyData);

        // 생성된 EnemyView 반환
        return enemyView;
    }
}
