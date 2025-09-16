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

    [SerializeField] private LocalizedText descTxt;

    void Start()
    {
        descTxt.Clear();
        characterSelector.Init("character", descTxt);
        difficultySelector.Init("difficulty", descTxt);

        startButton.onClick.AddListener(onClickStart);
    }

    void onClickStart()
    {
        int selectedCh = characterSelector.SelectedIdx;
        int selectedDf = difficultySelector.SelectedIdx;

        // TODO이 값을 정보로 넘기고 시작하기
        SendDataToGm(selectedCh, selectedDf);

        UnityEngine.SceneManagement.SceneManager.LoadScene("StageScene");
    }

    private void SendDataToGm(int selCh, int selDf)
    {
        //난이도 설정
        GameManager.gameManager.playerData.difficulty = selDf;

        //캐릭터 설정
        CharacterData characterData = new CharacterData();

        switch (selCh)
        {
            case 0: //소환사
                characterData.name = "summoner";
                characterData.maxHp = 50;
                characterData.hp = 30;
                break;

            case 1: //블라디
                characterData.name = "bloodwitch";
                characterData.maxHp = characterData.hp = 50;
                break;

            case 2: // 버서커
                characterData.name = "berserker";
                characterData.maxHp = characterData.hp = 60;
                break;

            case 3: //방어전사
                characterData.name = "rammus";
                characterData.maxHp = characterData.hp = 70;
                break;
        }

        GameManager.gameManager.playerData.playerDeck = CardGenerator.LoadDeck(-1, 0, selCh); //기본덱 셋팅
        characterData.baseShield = 0;
        GameManager.gameManager.playerData.characterData = characterData;
    }
}
