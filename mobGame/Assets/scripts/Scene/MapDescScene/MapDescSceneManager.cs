using UnityEngine;
using UnityEngine.UI;

public class MapDescSceneManager : MonoBehaviour
{
    [SerializeField] private Image BackImage;
    [SerializeField] private LocalizedText DescTxt;

    private int maxIndex = 3;
    private int currIndex = 1;
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            ChangeDescription();
        }
    }

    private void ChangeDescription()
    {
        if (currIndex > maxIndex)
        {
            //TODO 씬전환
            return;
        }

        //배경 교체
        if (currIndex == 3)
        {
            BackImage.sprite = BackgroundLoader.LoadBackgroundSprite(6, isMap: true);
        }

        MapTheme theme = GameManager.gameManager.playerData.currentMap.theme;
        string key = $"MapDesc_{theme.ToString().ToLower()}_{currIndex}";

        DescTxt.SetText(key);

        currIndex++;
    }
}
