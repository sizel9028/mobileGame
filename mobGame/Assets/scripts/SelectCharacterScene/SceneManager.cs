using UnityEngine;
using UnityEngine.UI;

public class NewMonoBehaviourScript : MonoBehaviour
{
    //캐릭터 선택씬만 관리하는 매니저, 캐릭터 선택시 주변을 황금색으로 바꿈

    // 캐릭터 버튼,이미지
    public Button[] characterButtons;
    public Image[] characterImages;

    //선택 인덱스, 색
    private int selectedIdx = -1;
    private Color selectedColor = new Color(1f, 0.84f, 0f);
    private Color defaultColor = Color.black;

    public Button startButton;

    void Start()
    {
        for (int i = 0; i < characterButtons.Length; ++i)
        {
            int index = i;
            characterButtons[i].onClick.AddListener(() => OnSelect(index));
        }

        startButton.onClick.AddListener(OnClickStart);
    }

    void OnSelect(int index)
    {
        if (selectedIdx != -1)
        {
            characterImages[selectedIdx].color = defaultColor;
        }

        characterImages[index].color = selectedColor;
        selectedIdx = index;
    }

    void OnClickStart()
    {
        if (selectedIdx == -1)
        {
            return;
        }

        //TODO 선택된 캐릭터의 정보값을 넘김 + 다음 씬 이동
    }
}
