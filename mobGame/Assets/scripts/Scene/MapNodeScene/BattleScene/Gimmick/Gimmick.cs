using System;
using UnityEngine;


[Serializable]
public class Gimmick
{
    public int gimmicCount;  //몇번까지 발동
    public string gimmickName; //기믹 이름
    public float gimmicCondition;  // 기믹 발동조건의 수치 담당

    public Gimmick(string name, float condition, int count)
    {
        this.gimmickName = name;
        this.gimmicCondition = condition;
        this.gimmicCount = count;
    }
}
