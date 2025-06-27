using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

public class ChSceneManager : MonoBehaviour
{
    //캐릭터 선택씬만 관리하는 매니저, 캐릭터 선택시 주변을 황금색으로 바꿈

    // 캐릭터 버튼,이미지
    public SelectUI characterSelector;
    public SelectUI difficultySelector;
    public Button startButton;

    void Start()
    {
        characterSelector.Init();
        difficultySelector.Init();

        startButton.onClick.AddListener(onClickStart);
    }

    void onClickStart()
    {
        int selectedCh = characterSelector.SelectedIdx;
        int selectedDf = difficultySelector.SelectedIdx;

        // TODO이 값을 정보로 넘기고 시작하기

        UnityEngine.SceneManagement.SceneManager.LoadScene("StageScene");
    }
}
