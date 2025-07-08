using System.Collections.Generic;
using UnityEngine;


//CharacterData/data.csv의 파일을 읽어 모든 캐릭터의 정보를 dict에 저장
public class ChManager : MonoBehaviour
{
    public static ChManager chManager;
    private Dictionary<string, CharacterData> chDict = new();

    void Awake()
    {
        if (chManager == null)
        {
            chManager = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        LoadCharacters();
    }

    private void LoadCharacters()
    {
        TextAsset csvFile = Resources.Load<TextAsset>("CharacterData/data");
        if (csvFile == null)
        {
            Debug.LogError("캐릭터 CSV 파일이 없습니다.");
            return;
        }

        string[] lines = csvFile.text.Split('\n');
        for (int i = 1; i < lines.Length; ++i)
        {
            string line = lines[i].Trim();
            if (string.IsNullOrWhiteSpace(line)) continue;

            string[] parts = line.Split(',');
            if (parts.Length < 4) continue;

            string key = parts[0];

            CharacterData data = new CharacterData
            {
                name = parts[1],
                hp = int.Parse(parts[2]),
                baseShield = int.Parse(parts[3])
            };

            if (chDict.ContainsKey(key))
            {
                Debug.LogWarning($"[ChManager] 중복된 ID: {key}");
                continue;
            }

            chDict[key] = data;

        }
    }

    public CharacterData GetCharacterData(int stage, int theme, int level)   // stage-theme-level(1-1-1)이 id를 의미
    {
        string key = $"{stage}-{theme}-{level}";
        if (chDict.TryGetValue(key, out var data))
        {
            return data;
        }

        Debug.LogWarning($"[ChManager] ID {key}에 해당하는 캐릭터 없음");
        return null;
    }
}
