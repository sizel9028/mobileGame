using UnityEngine;
using UnityEngine.SceneManagement;

public class EndGameSceneManager : MonoBehaviour
{
    //endGame시 오는 씬

    void Update()
    {
        //컴퓨터 터치 전용
        if (Input.GetMouseButtonDown(0))
        {
            OnEndGameClick();
        }

        //모바일 전용
        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
        {
            OnEndGameClick();
        }
    }
    

    private void OnEndGameClick()
    {
        // 저장된 플레이어 데이터 삭제
        SaveManager.saveManager.DeletePlayer();

        // startScene으로 이동
        SceneManager.LoadScene("startScene");
    }

}
