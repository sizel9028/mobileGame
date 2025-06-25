using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class StartMenu : MonoBehaviour
{
    //언어 버튼
    public Button[] langButtons;
    public string[] langCodes = { "ko", "en", "ja" };

    //버튼 모음
    public Button btnNewGame;
    public Button btnContinue;
    public Button btnExit;
    public Button btnLanguage;
    //언어 패널
    public GameObject langPanel;
    public GameObject quitMsg;


    void Start()
    {
        for (int i = 0; i < langButtons.Length; ++i)
        {
            int index = i;
            langButtons[i].onClick.AddListener(() =>
            {
                ChangeLanguage(langCodes[index]);
            });
        }

        btnNewGame.onClick.AddListener(onClickNewGame);
        btnContinue.onClick.AddListener(onClickContinue);
        btnExit.onClick.AddListener(onClickQuit);
        btnLanguage.onClick.AddListener(showLangPanel);

    }

    void ChangeLanguage(string langCodes)
    {
        if (langCodes == LocalizationManager.languageM.currentLanguage)
            return;

        //TODO 언어값을 데이터 저장소에 저장
            
        if (quitMsg != null)
            quitMsg.SetActive(true);

        StartCoroutine(quitDelay(2f));

    }

    private IEnumerator quitDelay(float delay)
    {
        yield return new WaitForSeconds(delay);

        Application.Quit();
    }

    void onClickNewGame()
    {
        //TODO 저장소에 새로운 값을 채움 + 씬 변환
    }

    void onClickContinue()
    {
        //TODO 저장소에서 이전 값 + 씬
    }

    void onClickQuit()
    {
        Application.Quit();
    }

    void showLangPanel()
    {
        if (langPanel != null)
        {
            langPanel.SetActive(!langPanel.activeSelf);
        }
    }
}
