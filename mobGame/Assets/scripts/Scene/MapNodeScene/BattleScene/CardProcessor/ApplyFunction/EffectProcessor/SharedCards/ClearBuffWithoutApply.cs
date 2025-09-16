using System.Collections;
using UnityEngine;

public partial class CardEffectProcessor
{
    private void ApplyClearBuffWithoutApply(float amount)
    {
        targetUI.character.effectCardManager.ClearWithoutApply();
    }
}