using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

public class NewMonoBehaviourScript : MonoBehaviour
{
    //캐릭터 선택씬만 관리하는 매니저, 캐릭터 선택시 주변을 황금색으로 바꿈

    // 캐릭터 버튼,이미지
    public Button[] characterButtons;
    public Image[] characterImages;
    
    // 난이도 버튼,이미지
    public Button[] DifficultyButtons;
    public Image[] DifficultyImages;

    //선택 인덱스, 색
    private int selectedChIdx = 0;
    private int selectedDfIdx = 0;
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

        for (int i = 0; i < DifficultyButtons.Length; ++i)
        {
            int index = i;
            DifficultyButtons[i].onClick.AddListener(() => OnDfSelect(index));
        }

        startButton.onClick.AddListener(OnClickStart);
        
        characterImages[selectedChIdx].color = selectedColor;
        DifficultyImages[selectedDfIdx].color = selectedColor;
    }

    void OnSelect(int index)
    {
        characterImages[selectedChIdx].color = defaultColor;
        characterImages[index].color = selectedColor;
        selectedChIdx = index;
    }

    void OnDfSelect(int index)
    {
        DifficultyImages[selectedDfIdx].color = defaultColor;
        DifficultyImages[index].color = selectedColor;
        selectedDfIdx = index;
    }

    void OnClickStart()
    {

        //TODO 선택된 캐릭터의 정보값을 넘김 + 다음 씬 이동
    }
}
