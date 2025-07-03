using TMPro;
using UnityEngine;

public class LocalizedText : MonoBehaviour
{
    public string key;
    private TMP_Text text;

    void Awake()
    {
        text = GetComponent<TMP_Text>();
    }


    void Start()
    {
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

}
