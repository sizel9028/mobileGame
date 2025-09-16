using System.Collections.Generic;

public partial class GimmickManager
{
    private void PlaySummon(Character character, Gimmick gimmick)
    {
        switch (gimmick.gimmicCount)
        {
            case 1:
                //Debug.Log("소환 기믹 적용");
                CharacterUIManager.Instance.AddCharacterByName("NOP_NORMAL_1", character.isPlayer);
                break;
        }

        gimmick.gimmicCount = 0;
    }
}
