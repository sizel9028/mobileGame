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
        //TODO 게임 시작
    }

    void OnBackClick()
    {
        //TODO 이전화면으로 돌아가기
    }
}
