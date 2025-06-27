using UnityEngine;
using UnityEngine.UI;

public class SelectUI : MonoBehaviour
{
    //선택시 테두리가 황금색이 됨
    public Button[] buttons;
    public Image[] images;

    private int selectedIdx = 0;
    private Color selectedColor = new Color(1f, 0.84f, 0f);
    private Color defaultColor = Color.black;
    // 읽기전용
    public int SelectedIdx => selectedIdx;

    public void Init()
    {
        for (int i = 0; i < buttons.Length; ++i)
        {
            int idx = i;
            buttons[i].onClick.AddListener(() => Select(idx));
        }

        if (images.Length > 0)
            images[0].color = selectedColor;
    }

    public void Select(int index)
    {
        images[selectedIdx].color = defaultColor;
        images[index].color = selectedColor;
        selectedIdx = index;    
    }

}
