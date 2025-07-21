using System.Collections.Generic;
using UnityEngine;

public static class StageLoader
{
    public static List<string> Load(int stageNumber, int level)
    {
        MapTheme theme = GameManager.gameManager.playerData.currentMap.theme;
        string themeName = theme.ToString();

        string path = $"BattleData/{themeName}/stage{stageNumber}";
        TextAsset csv = Resources.Load<TextAsset>(path);

        if (csv == null)
        {
            Debug.LogError($"[StageLoader] CSV 파일 로드 실패: {path}");
            return null;
        }

        string[] lines = csv.text.Split('\n');

        int targetLine = level; // 실제 인덱스는 level (줄 번호 그대로)

        if (targetLine >= lines.Length)
        {
            Debug.LogWarning($"[StageLoader] Level {level}은 존재하지 않음: {path}");
            return null;
        }

        string line = lines[targetLine].Trim();
        if (string.IsNullOrWhiteSpace(line)) return new List<string>();

        string[] tokens = line.Split(',');
        var result = new List<string>();
        foreach (var token in tokens)
        {
            result.Add(token.Trim());
        }

        return result;
    }
}
