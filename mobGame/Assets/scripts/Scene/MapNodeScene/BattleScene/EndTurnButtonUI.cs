using UnityEngine;

public class EndTurnButtonUI : MonoBehaviour
{
    [SerializeField] private EnemySystem enemySystem;
    [SerializeField] private EnemyBoardView enemyBoardView;

    public void Onclick()
    {
        Debug.Log("End Turn 버튼 클릭됨"); // 로그 확인용


        // 액션 시스템 제거 → 직접 호출
        enemySystem.StartEnemyTurn();
        Debug.Log($"현재 EnemyView 개수: {enemyBoardView.EnemyViews.Count}");
    }
}
