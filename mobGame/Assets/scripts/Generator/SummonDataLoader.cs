using System;
using System.Collections.Generic;
using UnityEngine;


public static class SummonDataLoader
{

    //리소스/SummonData/summonData.csv 에서 string값을 받아옴   
    public static string GetName(float amount)
    {
        TextAsset ta = Resources.Load<TextAsset>("SummonData/summonData");
        if (ta == null)
        {
            Debug.LogError("summonData.csv 파일을 찾을 수 없습니다.");
            return null;
        }

        string[] lines = ta.text.Split(new[] { '\n', '\r' }, System.StringSplitOptions.RemoveEmptyEntries);

        // 1부터 시작하는 줄 번호
        int index = Mathf.Clamp(Mathf.FloorToInt(amount), 0, lines.Length - 1);

        return lines[index].Trim();
    }

    public static string GetFusionResult(string name1, string name2, int currentLevel)
    {
        TextAsset ta = Resources.Load<TextAsset>("SummonData/fusionData");
        if (ta == null)
        {
            Debug.LogError("fusionData.csv 파일을 찾을 수 없습니다.");
            return null;
        }

        string[] lines = ta.text.Split(new[] { '\n', '\r' }, System.StringSplitOptions.RemoveEmptyEntries);

        foreach (string raw in lines)
        {
            string[] tokens = raw.Split(',');
            if (tokens.Length < 3) continue; // 최소 3칸은 있어야 함

            string mat1 = tokens[0].Trim();
            string mat2 = tokens[1].Trim();
            string result = tokens[2].Trim();
            int requiredLevel = 0;

            if (tokens.Length >= 4)
                int.TryParse(tokens[3].Trim(), out requiredLevel);

            // 순서 무관 비교
            if ((mat1 == name1 && mat2 == name2) || (mat1 == name2 && mat2 == name1))
            {
                if (currentLevel < requiredLevel)
                {
                    Debug.Log($"[Fusion] 레벨 부족: 필요 {requiredLevel}, 현재 {currentLevel}");
                    return null;
                }
                return result;
            }
        }

        return null; // 조합 없음
    }
}