using System.Collections.Generic;
using UnityEngine;

public static class GimmickLoader
{
    public static List<Gimmick> GetGimmickByName(string characterName)
    {
        TextAsset csv = Resources.Load<TextAsset>("Gimmick/gimmicks");
        if (csv == null)
        {
            Debug.LogError("[GimmickLoader] gimmicks.csv 파일을 찾을 수 없습니다.");
            return new List<Gimmick>();
        }

        var lines = csv.text.Split('\n');
        foreach (var rawLine in lines)
        {
            string line = rawLine.Trim();
            if (string.IsNullOrEmpty(line)) continue;

            string[] tokens = line.Split('|');
            if (tokens.Length < 2) continue;

            string name = tokens[0].Trim();
            if (name != characterName) continue;

            List<Gimmick> gimmicks = new();

            for (int i = 1; i < tokens.Length; i++)
            {
                var parts = tokens[i].Split(new[] { "::" }, System.StringSplitOptions.None);
                if (parts.Length != 2) continue;

                string gimmickName = parts[0].Trim();
                var values = parts[1].Split(',');

                if (values.Length != 2) continue;

                if (float.TryParse(values[0], out float condition) && int.TryParse(values[1], out int count))
                {
                    gimmicks.Add(new Gimmick(gimmickName, condition, count));
                }
                else
                {
                    Debug.LogWarning($"[GimmickLoader] 파싱 실패: {tokens[i]}");
                }
            }

            return gimmicks;
        }

        return new List<Gimmick>();
    }

}
