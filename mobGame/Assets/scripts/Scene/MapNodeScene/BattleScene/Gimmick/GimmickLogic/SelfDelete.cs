using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//소환수를 죽이고 다른 소환을 시킴
public partial class GimmickManager
{
    private void PlaySelfDelete(Character character, Gimmick gimmick)
    {

        var characterUI = CharacterUIManager.Instance.GetUI(character);

        if (characterUI != null)
        {
            if (character.isPlayer)
            {
                int idx = CharacterUIManager.Instance.playerUIs.IndexOf(characterUI);
                if (idx >= 0) CharacterUIManager.Instance.playerUIs[idx] = null;
            }
            else
            {
                int idx = CharacterUIManager.Instance.enemyUIs.IndexOf(characterUI);
                if (idx >= 0) CharacterUIManager.Instance.enemyUIs[idx] = null;
            }

            characterUI.DestroySelf();

            Battle.Instance.StartCoroutine(PlaySummonRoutine(character, gimmick));
        }
    }

    private IEnumerator PlaySummonRoutine(Character character, Gimmick gimmick)
    {
        yield return new WaitForSeconds(0.5f);
        PlaySummon(character, gimmick);
    }
}
