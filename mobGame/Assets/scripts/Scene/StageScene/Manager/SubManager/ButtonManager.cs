using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ButtonManager : MonoBehaviour, IPushable
{
    public Button startButton;


    public MapSceneManager manager;

    void Start()
    {
        startButton.onClick.AddListener(OnStartClick);
    }


    void OnStartClick()
    {
        manager.HandleStart();
    }

    public void PushBtnBack()
    {
        SceneManager.LoadScene("startScene");
    }
    public void PushBtnInventory()
    {
        GameManager.gameManager.CardViewCards = (SceneType.InventoryScene, GameManager.gameManager.playerData.playerDeck.cards);
        SceneManager.LoadScene("CardViewerScene");
    }

}
