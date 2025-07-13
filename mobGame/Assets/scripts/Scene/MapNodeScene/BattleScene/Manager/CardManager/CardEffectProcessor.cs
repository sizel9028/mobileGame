using System.Collections.Generic;
using UnityEngine;

public class CardEffectProcessor : MonoBehaviour
{
    private Dictionary<string, bool> dirtyFlag = new();  //카드를 적용했을때 어떤 계수가 쓰였는지 체크
    // 공격 카드가 쓰인다면 공격 계수에 관한 string을 참으로 바꿈

    public bool GetDirtyFlag(string key)
    {
        if (dirtyFlag.TryGetValue(key, out bool val))
        {
            return val;
        }

        return false;
    }
}
