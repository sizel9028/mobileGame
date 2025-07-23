using UnityEngine;

//csv 파일을 읽어 CharacterData를 반환
public static class ChdataGenerator
{
    private const string CsvPath = "CharacterData/data"; //csv파일 위치

    public static CharacterData GetData(string name)
    {
        TextAsset csvFile = Resources.Load<TextAsset>(CsvPath);
        if (csvFile == null)
        {
            Debug.LogError($"[ChDataGenerator] CSV 파일을 찾을 수 없습니다: {CsvPath}");
            return null;
        }

        string[] lines = csvFile.text.Split('\n');

        for (int i = 1; i < lines.Length; i++) // 헤더 제외
        {
            if (string.IsNullOrWhiteSpace(lines[i])) continue;

            string[] tokens = lines[i].Split(',');

            if (tokens.Length < 4)
            {
                Debug.LogWarning($"[ChDataGenerator] 잘못된 줄: {lines[i]}");
                continue;
            }

            string entryName = tokens[0].Trim();
            if (entryName != name) continue;

            CharacterData data = new CharacterData
            {
                name = entryName,
                maxHp = int.Parse(tokens[1]),
                hp = int.Parse(tokens[2]),
                baseShield = int.Parse(tokens[3])
            };

            return data;
        }

        Debug.LogWarning($"[ChDataGenerator] '{name}'에 해당하는 캐릭터 데이터를 찾을 수 없습니다.");
        return null;
    }
}
