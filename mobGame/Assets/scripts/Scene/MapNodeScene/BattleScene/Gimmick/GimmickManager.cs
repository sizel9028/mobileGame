using System.Collections.Generic;
using UnityEngine;

//모든 기믹을 관리하는 클래스
public partial class GimmickManager : Singleton<GimmickManager>
{
    private CardEffectProcessor processor = new();  //기믹 효과도 일부는 카드처럼 작동함
    private CoefficientModifier modifier = new();


    //기믹중 사용가능하면 true를 반환, 아니면 false
    public bool CanActiveWithName(Character character, Gimmick gimmick)
    {
        Debug.Log($"[Gimmick] Name = {gimmick.gimmickName}, Condition = {gimmick.gimmicCondition}, Count = {gimmick.gimmicCount}");
        Debug.Log($"[기믹 조건 검사] 캐릭터 HP: {character.currentHp}");
        Debug.Log($"[DEBUG] gimmickName: '{gimmick.gimmickName}', EqualTo 'Summon'? → {gimmick.gimmickName == "Summon"}");

        switch (gimmick.gimmickName)
        {
            case "Summon":
                if (character.currentHp <= gimmick.gimmicCondition) return true;
                break;

            case "Explode":
                if (character.currentHp <= 0)
                {
                    if (Random.value <= gimmick.gimmicCondition) return true;
                }
                break;

            case "RageGain":
                return true;

            case "RageUse":
                if (character.statMultiplier.rage >= gimmick.gimmicCondition) return true;
                break;

            case "ReturnCorpse":
                if (character.currentHp <= 0) return true;
                break;

            case "Poison":
                return true;

            case "SelfDelete":
                float hpRatio = (float)character.currentHp / character.maxHp;
                if (hpRatio <= gimmick.gimmicCondition) return true;
                break;
        }

        Debug.Log("기믹 실행 안됨");
        return false;
    }

    public void PlayGimmick(Character character, Gimmick gimmick)
    {
        if (!CanActiveWithName(character, gimmick)) return;  //작동안함

        Debug.Log("기믹 실행 준비중");
        switch (gimmick.gimmickName)
        {
            case "Summon":
                PlaySummon(character, gimmick);
                break;

            case "Explode":
                PlayExplode(character, gimmick);
                break;

            case "RageGain":
                PlayRageGain(character, gimmick);
                break;

            case "RageUse":
                PlayRageUse(character, gimmick);
                break;

            case "ReturnCorpse":
                PlayReturnCorpse(character, gimmick);
                break;

            case "Poison":
                PlayPoison(character, gimmick);
                break;

            case "SelfDelete":
                PlaySelfDelete(character, gimmick);
                break;
        }
    }

    private void PlaySummon(Character character, Gimmick gimmick)
    {
        switch (gimmick.gimmicCount)
        {
            case 1:
                Debug.Log("소환 기믹 적용");
                CharacterUIManager.Instance.AddCharacterByName("Slime", character.isPlayer);
                break;
        }

        gimmick.gimmicCount = 0;
    }


    //캐릭터를 조사한다음에 카운트가 0이면 삭제시킴
    public void ClearGimmick(Character character)
    {
        for (int i = character.gimmicks.Count - 1; i >= 0; --i)
        {
            var gimmic = character.gimmicks[i];

            if (gimmic.gimmicCount == 0)
            {
                character.gimmicks.RemoveAt(i);
            }
        }
    }
    

}
