using TMPro;
using UnityEngine;

public class LocalizedText : MonoBehaviour
{
    public string key;
    private TMP_Text text;


    void Start()
    {
        text = GetComponent<TMP_Text>();
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
