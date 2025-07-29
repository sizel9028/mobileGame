using UnityEngine;

//모든 기믹을 관리하는 클래스
public class GimmickManager : Singleton<GimmickManager>
{


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
        }
    }

    //Count에 따라서 어떤걸 소환할지 하드코딩... ㅋㅋㅋ
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
