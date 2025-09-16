using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;


//게임 오버일경우 맵을 저장하지 않고 그냥 바로 스테이지 씬으로 넘김 
public class GameOverManager : MonoBehaviour
{
    void Awake()
    {
        //먼저 브금을 바꿈
        SoundManager.soundManager.PlayBGM(SoundManager.BGMType.Map);
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            GoToStageScene();
        }

        // 모바일 터치
        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
        {
            GoToStageScene();
        }
    }

    private void GoToStageScene()
    {
        SceneManager.LoadScene("stageScene");
    }

}
