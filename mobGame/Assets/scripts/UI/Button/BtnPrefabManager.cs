using System.Collections.Generic;
using UnityEngine;

public class BtnPrefabManager : MonoBehaviour
{
    [SerializeField] private ButtonPrefab buttonPrefab;
    [SerializeField] private Transform buttonParent;

    public List<ButtonType> buttonTypes;
    public List<RectTransform> buttonPos;

    [SerializeField] private MonoBehaviour processor; //버튼 처리 담당

    void Start()
    {
        CreateBtns();
    }

    private void CreateBtns()
    {
        for (int i = 0; i < buttonTypes.Count; ++i)
        {
            ButtonType type = buttonTypes[i];
            ButtonPrefab btn = Instantiate(buttonPrefab, buttonParent);
            btn.Setup(type, this);

            if (i < buttonPos.Count)
            {
                RectTransform rect = btn.GetComponent<RectTransform>();
                rect.anchoredPosition = buttonPos[i].anchoredPosition;
            }
        }
    }

    public void CreateBtn(ButtonType type, RectTransform position)
    {
        ButtonPrefab btn = Instantiate(buttonPrefab, buttonParent);
        btn.Setup(type, this);

        RectTransform rect = btn.GetComponent<RectTransform>();
        if (rect != null && position != null)
        {
            rect.anchoredPosition = position.anchoredPosition;
        }
    }

    //타입에 해당하는 버튼 전부 삭제
    public void RemoveBtnByType(ButtonType type)
    {
        List<Transform> toDestroy = new();

        foreach (Transform child in buttonParent)
        {
            ButtonPrefab btn = child.GetComponent<ButtonPrefab>();
            if (btn != null && btn.ButtonType == type)
            {
                toDestroy.Add(child);
            }
        }

        foreach (var t in toDestroy)
        {
            Destroy(t.gameObject);
        }
    }

    public void PushBtn(ButtonType buttonType)
    {
        if (processor is IPushable pushable)
        {
            switch (buttonType)
            {
                case ButtonType.Back:
                    pushable.PushBtnBack();
                    break;


                case ButtonType.Inventory:
                    pushable.PushBtnInventory();
                    break;

            }
        }
        else
        {
            Debug.LogWarning("[BtnPrefabManager] PushTarget이 IPushable이 아닙니다.");
        }
    }
}
