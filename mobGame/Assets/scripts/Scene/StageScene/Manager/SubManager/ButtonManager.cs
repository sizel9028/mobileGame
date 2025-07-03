using UnityEngine;
using UnityEngine.UI;

public class ButtonManager : MonoBehaviour
{
    public Button startButton;
    public Button backButton;

    public MapSceneManager manager;

    void Start()
    {
        startButton.onClick.AddListener(OnStartClick);
        backButton.onClick.AddListener(OnBackClick);
    }


    void OnStartClick()
    {
        manager.HandleStart();
    }

    void OnBackClick()
    {
        manager.HandleBack();
    }
}
