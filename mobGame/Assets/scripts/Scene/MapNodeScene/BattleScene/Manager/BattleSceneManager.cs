using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using UnityEngine.SceneManagement;


public enum TurnType
{
    PlayerTurn, EnemyTurn
}

public class Battle : Singleton<Battle>
{
    //BattleScene의 최상위 매니저
    [SerializeField] private EndTurnButtonUI endTurnBtn;
    public int turnCount { get; private set; }  //에너미 기준
    public bool isProcessingCard = false;
    public TurnType turnType;  // 누구 턴인지 체크


    void Start()
    {
        turnType = TurnType.PlayerTurn;
        isProcessingCard = false;
        turnCount = 0; //턴 초기화
        DeckManager.Instance.InitDeck(); //플레이어 덱을 세팅
        EnemyDeckManager.Instance.InitEnemyDeck(); //에너미 덱을 세팅

        PassiveProcessor.Instance.ApplyPassiveCard
        (
            DeckManager.Instance.passiveCards,
            EnemyDeckManager.Instance.passiveCards
        );

        EnemyManaSystem.Instance.InitMana();
        ManaSystem.Instance.InitManaSystem();  //마나 초기화 먼저

        //사용가능한 패 확인
        DeckManager.Instance.handView.CheckUsableCard();

        InitializeBattle(); //플레이어 적을 소환

    }

    //TODO 덱을 먼저 세팅후 그걸 계수에 영향을 가게 만들어야 할듯
    public void InitializeBattle()
    {
        //--- test --- //TODO 스테이 정보를 진짜 스테이지로 함
        int stageNumber = GameManager.gameManager.playerData.currentMap.stageNumber;
        //int[] level = LevelGenerator.GetLevelInfo(GameManager.gameManager.playerData.currentMap);

        //TODO 1대신 level을 넣음
        List<string> enemyNames = StageLoader.Load(stageNumber, 1);

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
        var chData = GameManager.gameManager.playerData.characterData;
        CharacterUIManager.Instance.AddCharacterByData(chData);
    }

    public IEnumerator EndGame(BattleResult result)
    {
        endTurnBtn.SetActive(false);
        //TODO 씬전환 + 정보넘김s
        yield return new WaitForSeconds(2f);

        if (result == BattleResult.PlayerWin)
        {
            GameManager.gameManager.endHp = CharacterUIManager.Instance.playerUIs[0].character.currentHp;
            Debug.Log("플레이어 승리");
            SceneManager.LoadScene("GachaScene");
        }
        else if (result == BattleResult.EnemyWin)
        {
            Debug.Log("에너미 승리");
            SceneManager.LoadScene("GameOverScene");
        }
        else
        {
            Debug.LogWarning("끝나지 않았는데 end 함수 호출");
            //예외 처리
            SceneManager.LoadScene("MapScene");
        }
    }

    public void PlayerTurn()
    {
        turnType = TurnType.PlayerTurn;
        MapEffectProcessor.Instance.ProcessMapEffect();  //플레이어 턴 시작때 맵의 효과를 받음
        endTurnBtn.SetActive(true);  //턴 버튼 UI 보여줌
    }

    public IEnumerator EnemyTurn()
    {
        CheckTurnEffects(true); // 플레이어 턴 체크
        //턴 관리
        turnType = TurnType.EnemyTurn;
        ++turnCount;

        endTurnBtn.SetActive(false); // 턴 버튼 UI 숨김

        EnemyManaSystem.Instance.refillMana();
        yield return EnemyDeckManager.Instance.PlayCard();  //적이 카드를 낸다


        ManaSystem.Instance.Refill();
        CheckTurnEffects(false); // 플레이어 턴 체크
        PlayerTurn(); //플레이어 턴으로 넘어감
    }

    //턴 기반 카드 횟수 체크
    /*private void CheckTurnEffects(bool isPlayerTurn)
    {
        var UIs = isPlayerTurn ? CharacterUIManager.Instance.playerUIs : CharacterUIManager.Instance.enemyUIs;

        foreach (var ui in UIs)
        {
            if (ui == null || ui.character == null) continue;

            ui.character.effectCardManager?.CheckTurn();
        }
    }*/

    //턴 기반 효과 처리 담당 (고정피해)
    private void CheckTurnEffects(bool isPlayerTurn)
    {
        var UIs = isPlayerTurn ? CharacterUIManager.Instance.playerUIs : CharacterUIManager.Instance.enemyUIs;

        foreach (var ui in UIs)
        {
            if (ui == null || ui.character == null) continue;

            // 카드 효과 체크
            ui.character.effectCardManager?.CheckTurn();

            // --- 재생/부패 체크 ---
            var sm = ui.character.statMultiplier;

            int addHp = Mathf.RoundToInt(sm.turnAddHp);
            int decHp = Mathf.RoundToInt(sm.turnDecreaseHp);

            var processor = new CardEffectProcessor();

            if (addHp > 0)
            {
                var healCard = new CardData
                {
                    effectMap = new Dictionary<string, float> { { "Heal", addHp } },
                    cardTarget = CardTarget.onePlayer
                };

                processor.ProcessCardEffect(healCard, null, new List<CharacterUI> { ui });
            }

            if (decHp > 0)
            {
                var dmgCard = new CardData
                {
                    effectMap = new Dictionary<string, float> { { "Damage", decHp } },
                    cardTarget = CardTarget.onePlayer
                };

                processor.ProcessCardEffect(dmgCard, null, new List<CharacterUI> { ui });
            }
        }
    }


}
