using UnityEngine;

public class GameManager : MonoBehaviour
{
    //게임 진행을 관리하는 매니저, 재화정보를 다룸

    public static GameManager gameManager;
    public PlayerData playerData;

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
    }

    public void LoadGame()
    {
        playerData = SaveManager.saveManager.Load();
    }

    public void StartNewGame()
    {
        playerData = SaveManager.saveManager.CreateNew();
    }
}
