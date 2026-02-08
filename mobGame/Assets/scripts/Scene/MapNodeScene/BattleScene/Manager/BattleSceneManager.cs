using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;



public enum TurnType
{
    PlayerTurn, EnemyTurn
}

public class Battle : Singleton<Battle>
{
    //BattleScene의 최상위 매니저
    [SerializeField] private EndTurnButtonUI endTurnBtn;
    [SerializeField] private Image backgroundImage;
    public int turnCount { get; private set; }  //에너미 기준
    public bool isProcessingCard = false;
    public TurnType turnType;  // 누구 턴인지 체크

    void Start()
    {
        //브금 셋팅
        var nodeId = GameManager.gameManager.nodeId;
        var type = GameManager.gameManager.playerData.currentMap.nodes[nodeId].nodeType == NodeType.Boss ?
        SoundManager.BGMType.EliteBattle : SoundManager.BGMType.Battle;
        SoundManager.soundManager.PlayBGM(type);

        int stage = GameManager.gameManager.playerData.currentMap.stageNumber;
        backgroundImage.sprite = BackgroundLoader.LoadBackgroundSprite(stage, isMap: false);

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
        int nodeId = GameManager.gameManager.nodeId;
        int level = LevelGenerator.GetLevelInfo(nodeId);

        //TODO 1대신 level을 넣음
        List<string> enemyNames = StageLoader.Load(stageNumber, level);

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

        SetPuck();
    }

    public IEnumerator EndGame(BattleResult result)
    {
        endTurnBtn.SetActive(false);

        //TODO 씬전환 + 정보넘김s
        yield return new WaitForSeconds(2f);

        if (result == BattleResult.PlayerWin)
        {
            //게임에서 승리하면 사용한 스크롤 카드 다 삭제
            DeleteUsedScrollCards();
            var chData = CharacterUIManager.Instance.playerUIs[0].character;
            GameManager.gameManager.playerData.characterData.hp = chData.currentHp -
            (int)PassiveProcessor.Instance.playerCh.statMultiplier.addHp; //카드 패시브로 증가한 맥스 hp는 뺌

            GameManager.gameManager.playerData.characterData.hp = Mathf.Max(1, GameManager.gameManager.playerData.characterData.hp);
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

    private void DeleteUsedScrollCards()
    {
        var playerDeck = GameManager.gameManager.playerData.playerDeck.cards;

        var remainingScrolls = new List<CardData>(DeckManager.Instance.scrollCards);

        for (int i = playerDeck.Count - 1; i >= 0; i--)
        {
            var card = playerDeck[i];
            if (card.cardType == CardType.Scroll)
            {
                if (remainingScrolls.Contains(card))
                {
                    // 살아남은 스크롤이면 리스트에서 하나만 제거 (중복 고려)
                    remainingScrolls.Remove(card);
                }
                else
                {
                    // 살아남은 리스트에 없는 스크롤 → 덱에서 제거
                    playerDeck.RemoveAt(i);
                }
            }
        }
    }

    public IEnumerator PlayerTurn()
    {
        turnType = TurnType.PlayerTurn;

        SetPuck();

        //DeckManager.Instance.ReDrawCards();  //패에 다시 카드를 넣음
        yield return DeckManager.Instance.ReDrawCards();
        MapEffectProcessor.Instance.ProcessMapEffect();  //플레이어 턴 시작때 맵의 효과를 받음

        ManaSystem.Instance.Refill();

        DeckManager.Instance.handView.CheckUsableCard();

        endTurnBtn.SetActive(true);  //턴 버튼 UI 보여줌
    }

    public IEnumerator EnemyTurn()
    {
        //턴 관리
        ++turnCount;
        turnType = TurnType.EnemyTurn;
        endTurnBtn.SetActive(false); // 턴 버튼 UI 숨김

        //모든 패를 버림
        DeckManager.Instance.handView.DiscardAllCards();

        yield return new WaitForSeconds(1f);  //패를 다 버릴때까지 딜레이

        CheckTurnEffects(false); // 에너미 턴 체크

        SetPuck();

        /*if (!(turnCount <= 1))
        {
            CheckTurnEffects(false); // 에너미 턴 체크
            yield return new WaitForSeconds(0.6f); // 데미지 입고 바로 카드 안씀
        }*/

        MapEffectProcessor.Instance.ProcessMapEffect();  //에너미 효과

        EnemyManaSystem.Instance.refillMana();
        yield return EnemyDeckManager.Instance.PlayCard();  //적이 카드를 낸다

        CheckTurnEffects(true);
        StartCoroutine(PlayerTurn()); //플레이어 턴으로 넘어감
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

    public void SetPuck()
    {
        SetPuckPlayer(true); SetPuckPlayer(false); 
    }

    private void SetPuckPlayer(bool isPlayer)
    {
        var UIs = isPlayer ? CharacterUIManager.Instance.playerUIs : CharacterUIManager.Instance.enemyUIs;

        foreach (var ui in UIs)
        {
            if (ui == null) continue;
            ui.SetPuck();
        }
    }


}
