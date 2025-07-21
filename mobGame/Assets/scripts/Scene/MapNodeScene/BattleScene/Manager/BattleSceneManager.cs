using UnityEngine;
using System.Collections.Generic;

public class Battle : MonoBehaviour
{
    //BattleScene의 최상위 매니저

    public DeckManager deckManager;


    void Start()
    {
        ManaSystem.Instance.InitManaSystem();  //마나 초기화 먼저
        
        InitializeBattle(); //적을 세팅

        deckManager.InitDeck(); //플레이어 덱을 세팅
    }

    public void InitializeBattle()
    {
        //--- test --- //TODO 스테이 정보를 진짜 스테이지로 함
        List<string> enemyNames = StageLoader.Load(1, 1);

        if (enemyNames == null)
        {
            Debug.LogError("[Battle] StageLoader 실패");
            return;
        }

        foreach (var name in enemyNames)
        {
            CharacterUIManager.Instance.AddCharacterByName(name, false);
        }

        CharacterUIManager.Instance.AddCharacterByName(enemyNames[0], true);
    }
}
