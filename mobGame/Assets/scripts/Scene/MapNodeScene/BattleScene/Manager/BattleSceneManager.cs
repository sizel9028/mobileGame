using UnityEngine;
using System.Collections.Generic;

public class Battle : Singleton<Battle>
{
    //BattleScene의 최상위 매니저
    [SerializeField] private EndTurnButtonUI endTurnBtn;


    void Start()
    {
        ManaSystem.Instance.InitManaSystem();  //마나 초기화 먼저

        InitializeBattle(); //적을 세팅

        DeckManager.Instance.InitDeck(); //플레이어 덱을 세팅
    }

    //TODO 덱을 먼저 세팅후 그걸 계수에 영향을 가게 만들어야 할듯
    public void InitializeBattle()
    {
        //--- test --- //TODO 스테이 정보를 진짜 스테이지로 함
        List<string> enemyNames = StageLoader.Load(1, 1);

        if (enemyNames == null)
        {
            Debug.LogError("[Battle] StageLoader 실패");
            return;
        }

        //적 소환
        foreach (var name in enemyNames)
        {
            CharacterUIManager.Instance.AddCharacterByName(name, false);
        }

        //플레이어 소환
        var playerName = GameManager.gameManager.playerData.characterData.name;
        CharacterUIManager.Instance.AddCharacterByName(playerName, true);
    }

    public void EndGame()
    {
        //TODO 씬전환 + 정보넘김
    }

    public void PlayerTurn()
    {
        endTurnBtn.SetActive(true);  //턴 버튼 UI 보여줌
    }

    public void EnemyTurn()
    {
        endTurnBtn.SetActive(false); // 턴 버튼 UI 숨김

        PlayerTurn(); //플레이어 턴으로 넘어감
    }

}
