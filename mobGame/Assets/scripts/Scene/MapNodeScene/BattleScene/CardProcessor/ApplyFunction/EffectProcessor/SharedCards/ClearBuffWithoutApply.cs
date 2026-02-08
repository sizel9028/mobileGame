using System.Collections;
using UnityEngine;

public partial class CardEffectProcessor
{
    private void ApplyClearBuffWithoutApply(float amount)
    {
        var playerUIs = CharacterUIManager.Instance.playerUIs;
        var enemyUIs = CharacterUIManager.Instance.enemyUIs;
        foreach (var ui in playerUIs)
        {
            if (ui == null) continue;
            ui.character.effectCardManager.ClearWithoutApply();
        }
        foreach (var ui in enemyUIs)
        {
            if (ui == null) continue;
            ui.character.effectCardManager.ClearWithoutApply();
        }
    }
}