using System.Collections;
using UnityEngine;

public partial class CardEffectProcessor
{

    //현재 적용된 모든 버프/디버프를 해제
    private void ApplyClearBuffWithApply(float amount)
    {
        var playerUIs = CharacterUIManager.Instance.playerUIs;
        var enemyUIs = CharacterUIManager.Instance.enemyUIs;
        foreach (var ui in playerUIs)
        {
            if (ui == null) continue;
            ui.character.effectCardManager.ClearWithApply();
        }
        foreach (var ui in enemyUIs)
        {
            if (ui == null) continue;
            ui.character.effectCardManager.ClearWithApply();
        }
    }
    
}