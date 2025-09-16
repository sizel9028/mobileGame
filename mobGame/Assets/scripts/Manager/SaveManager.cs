using UnityEngine;
using UnityEngine.Windows;
using System.IO;
using Unity.VisualScripting;
using UnityEngine.SceneManagement;

public class SaveManager : MonoBehaviour
{
    //PlayerData 클래스의 정보를 저장하고 불러옴
    public static SaveManager saveManager;
    private string playerSavePath;
    private string runeSavePath;
    public bool isTutorialEnd = false;

    void Awake()
    {
        if (saveManager == null)
        {
            saveManager = this;
            DontDestroyOnLoad(gameObject);
            string basePath = Application.persistentDataPath;
            playerSavePath = Path.Combine(basePath, "save.json");
            runeSavePath = Path.Combine(basePath, "rune.json");
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void SavePlayer(PlayerData data)
    {
        string json = JsonUtility.ToJson(data, true);
        System.IO.File.WriteAllText(playerSavePath, json);
    }

    public PlayerData LoadPlayer()
    {
        if (System.IO.File.Exists(playerSavePath))
        {
            string json = System.IO.File.ReadAllText(playerSavePath);
            return JsonUtility.FromJson<PlayerData>(json);
        }
        else
        {
            return null;
        }
    }

    //데이터 삭제
    public void DeletePlayer()
    {
        if (System.IO.File.Exists(playerSavePath))
        {
            System.IO.File.Delete(playerSavePath);
            Debug.Log("플레이어 데이터 파일 삭제 완료");
        }
    }

    public void SaveRune(RuneData data)
    {
        string json = JsonUtility.ToJson(data, true);
        System.IO.File.WriteAllText(runeSavePath, json);
    }

    public RuneData LoadRune()
    {
        if (System.IO.File.Exists(runeSavePath))
        {
            string json = System.IO.File.ReadAllText(runeSavePath);
            RuneData runeData = JsonUtility.FromJson<RuneData>(json);
            return runeData;
        }
        else
        {
            return InitData.CreateNewRuneData();
        }
    }

    public void SaveAll()
    {
        MarkCurrentNodeCleared();
        SaveCharacterData();
        SavePlayer(GameManager.gameManager.playerData);
    }

    private void SaveCharacterData()
    {
        var chManager = CharacterUIManager.Instance;
        if (chManager == null) return;

        var playerUI = chManager.playerUIs[0];
        if (playerUI == null) return;

        var characterData = GameManager.gameManager.playerData.characterData;

        characterData.maxHp = playerUI.character.maxHp;
        characterData.hp = playerUI.character.currentHp;

    }

    public void MarkCurrentNodeCleared()
    {
        var map = GameManager.gameManager.playerData.currentMap;
        var nodeId = GameManager.gameManager.nodeId;

        if (nodeId >= 0 && nodeId < map.nodes.Count && map.nodes[nodeId] != null)
        {
            map.nodes[nodeId].nodeType = NodeType.Cleared;
            Debug.Log($"[SaveManager] 노드 {nodeId} 클리어 처리 완료");
        }
        else
        {
            Debug.LogWarning("[SaveManager] 유효하지 않은 노드 인덱스");
        }

        UpdateMap();
    }

    private void UpdateMap()
    {
        var map = GameManager.gameManager.playerData.currentMap;
        int difficulty = GameManager.gameManager.playerData.difficulty;

        if (map.IsAllCleared())
        {
            if (map.theme == MapTheme.NOP)
            {
                isTutorialEnd = true;
                var values = (MapTheme[])System.Enum.GetValues(typeof(MapTheme));
                int randomIndex = Random.Range(1, values.Length);
                MapTheme mapTheme = values[randomIndex];
                //랜덤 맵 테마를 저장시킴
                GameManager.gameManager.playerData.currentMap.theme = mapTheme;

                MapNode nextRandomMap = MapGenerator.LoadMap(1, (int)mapTheme);  //현재는 첫번째 theme으로 고정 추후 랜덤값 사용
                GameManager.gameManager.playerData.currentMap = nextRandomMap;
                GameManager.gameManager.nodeId = 0;
                return;
            }
            
            int nextStage = map.stageNumber + 1;

            //Debug.Log(nextStage - difficulty - 1);
            if (nextStage > difficulty + 3)
            {
                Debug.Log("게임 종료, 스타트 씬 이동");
                //TODO 게임 종료 씬으로 이동
                SceneManager.LoadScene("EndGame");
                return;
            }

            int theme = (int)map.theme;
            MapNode nextMap = MapGenerator.LoadMap(nextStage, theme);
            //맵 이동
            GameManager.gameManager.playerData.currentMap = nextMap;
            GameManager.gameManager.nodeId = 0;
        }
    }
}
