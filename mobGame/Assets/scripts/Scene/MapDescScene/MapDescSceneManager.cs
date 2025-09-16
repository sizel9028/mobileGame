using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MapDescSceneManager : MonoBehaviour
{
    [SerializeField] private Image BackImage;
    [SerializeField] private LocalizedText DescTxt;

    private int maxIndex = 2;
    private int currIndex = 1;

    void Awake()
    {
        BackImage.color = Color.black;
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            ChangeDescription();
        }

        // 모바일 터치
        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
        {
            ChangeDescription();
        }
    }

    private void ChangeDescription()
    {
        if (currIndex > maxIndex)
        {
            //TODO 씬전환
            SceneManager.LoadScene("stageScene");
            return;
        }

        //배경 교체
        if (currIndex == 3)
        {
            //배경을 보여줌(검은색 > 흰색)
            //BackImage.color = Color.white;
            //BackImage.sprite = BackgroundLoader.LoadBackgroundSprite(6, isMap: true);
        }

        MapTheme theme = GameManager.gameManager.playerData.currentMap.theme;
        string key = $"MapDesc_{theme.ToString().ToLower()}_{currIndex}";

        DescTxt.SetText(key);

        currIndex++;
    }
}
