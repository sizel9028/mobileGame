using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    //게임 진행을 관리하는 매니저, 재화정보를 다룸

    public static GameManager gameManager;
    public PlayerData playerData;
    public RuneData runeData;

    public (SceneType sceneType, List<CardData> cards) CardViewCards;  //덱을 보여줄때 쓸 정보
    public List<CardData> shopSceneCards; // 샵씬에서 카드 제거로 넘어가는중 임시로 저장하는 씬정보
    public int maxReset;
    public int nodeId;  //노드 실행시 현재 실행 노드저장
    public int endHp; // 게임이 끝나고 playerHp저장 공간
    public bool CfSceneUpgrade = false; //campfire씬에서 강화를 했는지 체크

    public int buyDeleteCard = 0;  //상점씬에서 딜리트 카드를 얼만큼 구매했는지
    void Awake()
    {
        if (gameManager == null)
        {
            gameManager = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        shopSceneCards = null;
    }

    //룬 정보 불러오기
    void Start()
    {
        maxReset = 0;
        runeData = SaveManager.saveManager.LoadRune();
    }

    public void LoadGame()
    {
        playerData = SaveManager.saveManager.LoadPlayer();
    }

    public void StartNewGame()
    {
        playerData = InitData.CreateNewPlayerData();

        if (playerData == null)
        {
            Debug.LogWarning("[StartNewGame] character is NULL in playerData");
        }
        else
        {
            Debug.Log($"[StartNewGame] character 초기화 완료");
        }
    }
}
