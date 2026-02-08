using System.Collections.Generic;
using UnityEngine;

public partial class GimmickManager
{
    private void PlayRegeneration(Character character, Gimmick gimmick)
    {
        // 특정 hp 이하로 내려가면 Count만큼 hp를 회복시킴

        character.currentHp += gimmick.gimmicCount;
        character.currentHp = Mathf.Min(character.currentHp, character.maxHp);

        var ui = CharacterUIManager.Instance.GetUI(character);

        if (ui != null)
        {
            ui.Setup();
        }

        gimmick.gimmicCount = 0; //기믹을 한번만 적용하면 끝남
    }
}
