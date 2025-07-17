using UnityEngine;
using System.Collections.Generic;

public class Battle : MonoBehaviour
{
    //BattleScene의 최상위 매니저

    public DeckManager deckManager;
    [SerializeField] private EnemySystem enemySystem;
    [SerializeField] private Sprite testSprite;



    void Start()
    {
        var testList = new List<EnemyData>
    {
        new EnemyData { health = 30, sprite = testSprite },
        // 필요 시 여러 개 추가
    };

        enemySystem.Setup(testList);
        deckManager.InitDeck();
    }
}
