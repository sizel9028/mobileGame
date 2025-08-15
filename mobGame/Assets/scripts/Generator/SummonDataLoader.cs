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
}