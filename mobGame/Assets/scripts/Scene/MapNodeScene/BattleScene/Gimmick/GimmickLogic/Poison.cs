using System.Collections.Generic;
using UnityEngine;

public partial class GimmickManager
{
    //
    private void PlayPoison(Character character, Gimmick gimmick)
    {
        int damage = Mathf.RoundToInt(gimmick.gimmicCondition);


        // UI 갱신
        var targetUI = CharacterUIManager.Instance.GetUI(character);
        CardData tmpCard = new CardData
        {
            effectMap = new Dictionary<string, float> { { "Damage", damage } },
            cardTarget = CardTarget.oneEnemy
        };

        if (targetUI != null)
        {
            processor.ProcessCardEffect(tmpCard, null, new List<CharacterUI> { targetUI });
        }

        gimmick.gimmicCount--;
    }
}
