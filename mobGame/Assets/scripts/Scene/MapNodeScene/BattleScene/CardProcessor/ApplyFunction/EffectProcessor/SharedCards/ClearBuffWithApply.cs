using System.Collections;
using UnityEngine;

public partial class CardEffectProcessor
{

    //현재 적용된 모든 버프/디버프를 해제
    private void ApplyClearBuffWithApply(float amount)
    {
        targetUI.character.effectCardManager.ClearWithApply();
    }
    
}