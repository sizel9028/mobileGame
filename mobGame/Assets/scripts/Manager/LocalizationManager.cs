using System.Collections.Generic;
using TMPro;
using UnityEngine;

//언어관리

public class LocalizationManager : MonoBehaviour
{
    public static LocalizationManager languageM;

    private Dictionary<string, string> localizedText;
    public string currentLanguage = "ko"; // 영어 기본값

    public TMP_FontAsset fontKorean;
    public TMP_FontAsset fontEnglish;

    void Awake()
    {
        if (languageM == null)
        {
            languageM = this;
            DontDestroyOnLoad(gameObject);
            LoadLanguage(currentLanguage);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void LoadLanguage(string langCode) //currentLanguage에 따라 버튼 언어를 csv파일로부터 읽어옴
    {
        localizedText = new Dictionary<string, string>();

        TextAsset csv = Resources.Load<TextAsset>("localization");
        var lines = csv.text.Split('\n');   // lines는 하나의 줄(엔터로 구분)

        int langIdx = -1;
        var header = lines[0].Trim().Split(',');

        for (int i = 0; i < header.Length; ++i)
        {
            if (header[i].Trim() == langCode)
            {
                langIdx = i;
                break;
            }
        }

        if (langIdx == -1)
        {
            langIdx = 2;   //에러 발생시 무조건 영어로
        }

        for (int i = 1; i < lines.Length; ++i)
        {
            var row = lines[i].Trim().Split(',');
            if (row.Length > langIdx)
            {
                string key = row[0].Trim();
                string value = row[langIdx].Trim();
                localizedText[key] = value;
            }
        }
    }

    public string GetText(string key)
    {
        return localizedText.TryGetValue(key, out var value) ? value : $"#{key}#";
    }

    public TMP_FontAsset GetFont()
    {
        //TODO 폰트 넣기
        switch (currentLanguage)
        {
            case "ko": return fontKorean;
            case "en":
            default: return fontEnglish;
        }
    }
}
