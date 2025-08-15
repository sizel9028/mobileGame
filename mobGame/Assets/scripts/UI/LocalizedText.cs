using TMPro;
using UnityEngine;

public class LocalizedText : MonoBehaviour
{
    public string key;
    private TMP_Text text;
    public bool skipStart = false;

    void Awake()
    {
        text = GetComponent<TMP_Text>();
    }


    void Start()
    {
        if (skipStart) return;
        //text = GetComponent<TMP_Text>();
        if (LocalizationManager.languageM != null)
        {
            text.text = LocalizationManager.languageM.GetText(key);
            text.font = LocalizationManager.languageM.GetFont();
        }
        else
        {
            Debug.LogWarning("LocalizationManager가 존재하지 않습니다. 키: " + key);
        }
    }

    public void SetText(string newKey)
    {
        key = newKey;
        if (LocalizationManager.languageM != null)
        {
            text.text = LocalizationManager.languageM.GetText(key);
            text.font = LocalizationManager.languageM.GetFont();
        }
        else
        {
            Debug.LogWarning("LocalizationManager가 존재하지 않습니다. 키: " + key);
        }
    }

    public void AppendText(string extra)
    {
        if (text != null)
        {
            text.text += extra;
        }
    }

    public void Clear()
    {
        if (text != null)
        {
            text.text = "";
            text.font = LocalizationManager.languageM.GetFont();
        }
    }

}
