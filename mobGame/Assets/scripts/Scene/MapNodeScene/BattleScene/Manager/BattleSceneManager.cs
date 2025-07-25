using UnityEngine;
using System.Collections.Generic;

public class Battle : Singleton<Battle>
{
    //BattleScene의 최상위 매니저
    [SerializeField] private EndTurnButtonUI endTurnBtn;
    public int turnCount { get; private set; }  //에너미 기준


    void Start()
    {
        turnCount = 0; //턴 초기화
        EnemyManaSystem.Instance.InitMana();
        ManaSystem.Instance.InitManaSystem();  //마나 초기화 먼저
        DeckManager.Instance.InitDeck(); //플레이어 덱을 세팅
        EnemyDeckManager.Instance.InitEnemyDeck(); //에너미 덱을 세팅

        PassiveProcessor.Instance.ApplyPassiveCard
        (
            DeckManager.Instance.passiveCards,
            EnemyDeckManager.Instance.passiveCards
        );

        InitializeBattle(); //플레이어 적을 소환

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
        CheckTurnEffects(true); // 플레이어 턴 체크
    }

    public void EnemyTurn()
    {
        ++turnCount;
        endTurnBtn.SetActive(false); // 턴 버튼 UI 숨김
        CheckTurnEffects(false); // 플레이어 턴 체크

        EnemyManaSystem.Instance.refillMana();
        EnemyDeckManager.Instance.PlayCard();  //적이 카드를 낸다

        ManaSystem.Instance.Refill();
        PlayerTurn(); //플레이어 턴으로 넘어감
    }

    //턴 기반 카드 횟수 체크
    private void CheckTurnEffects(bool isPlayerTurn)
    {
        var UIs = isPlayerTurn ? CharacterUIManager.Instance.playerUIs : CharacterUIManager.Instance.enemyUIs;

        foreach (var ui in UIs)
        {
            if (ui == null || ui.character == null) continue;

            ui.character.effectCardManager?.CheckTurn();
        }
    }

}
