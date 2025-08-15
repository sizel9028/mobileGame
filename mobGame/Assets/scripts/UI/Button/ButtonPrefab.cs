using UnityEngine;
using UnityEngine.UI;

public enum ButtonType
{
    Back, Inventory
}
public class ButtonPrefab : MonoBehaviour
{
    [SerializeField] private Button button;
    private ButtonType buttonType;  //어떤 버튼인지 저장
    public ButtonType ButtonType => buttonType;

    private BtnPrefabManager manager;
    

    void Awake()
    {
        if (button != null)
        {
            button.onClick.AddListener(OnClick);
        }
    }

    public void Setup(ButtonType type, BtnPrefabManager manager)
    {
        buttonType = type;
        this.manager = manager;
        LoadArt();
    }

    void OnClick()
    {
        manager.PushBtn(buttonType);
    }

    private void LoadArt()
    {
        string path = $"Btn/{buttonType}";
        Sprite art = Resources.Load<Sprite>(path);

        if (art != null)
        {
            button.image.sprite = art;
        }
        else
        {
            Debug.LogWarning($"[ButtonPrefab] 버튼 아트 로드 실패: {path}");
        }
    }
}
