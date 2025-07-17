using UnityEngine;
using System.Collections.Generic;

public class Battle : MonoBehaviour
{
    //BattleScene의 최상위 매니저

    public DeckManager deckManager;
    [SerializeField] private EnemySystem enemySystem;
    [Header("전투 데이터")]
    [SerializeField] private EnemyBoardView enemyBoardView;

    [SerializeField] private List<EnemyData> testEnemies; // 인스펙터에서 세팅할 수도 있고, 코드에서 만들 수도 있음


    void Start()
    {
        InitializeBattle();

        deckManager.InitDeck();
    }
    /// <summary>
    /// 전투 초기화: 적 생성 및 배치
    /// </summary>
    public void InitializeBattle()
    {
        foreach (EnemyData enemy in testEnemies)
        {
            enemyBoardView.AddEnemy(enemy);
        }

        Debug.Log("[BattleSceneManager] 전투 초기화 완료");
    }
}
