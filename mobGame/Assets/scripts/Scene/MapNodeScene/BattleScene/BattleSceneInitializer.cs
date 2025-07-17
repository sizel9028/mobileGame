using System.Collections.Generic;
using UnityEngine;

public class BattleSceneInitializer : MonoBehaviour
{
    [Header("초기 적 데이터 세팅")]
    [SerializeField] private EnemyBoardView enemyBoardView;

    [SerializeField] private Sprite testEnemySprite1;
    [SerializeField] private Sprite testEnemySprite2;
    [SerializeField] private Sprite testEnemySprite3;

    void Start()
    {
        InitializeEnemies();
    }

    void InitializeEnemies()
    // 테스트용 코드
    {
        Debug.Log("[BattleSceneInitializer] 전투 초기화 시작");

        List<EnemyData> testEnemies = new()
        {
            new EnemyData { health = 30, sprite = testEnemySprite1 },
            new EnemyData { health = 50, sprite = testEnemySprite2 },
            new EnemyData { health = 80, sprite = testEnemySprite3 },
        };

        foreach (var enemy in testEnemies)
        {
            enemyBoardView.AddEnemy(enemy);
        }

        Debug.Log("[BattleSceneInitializer] 적 생성 완료");
    }
}
