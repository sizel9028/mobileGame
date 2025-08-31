using System.Collections;
using UnityEngine;

public partial class CardEffectProcessor
{
    private void DiceDamage()
    {
        int diceRoll = Random.Range(1, 7);
        float absoluteLuck = StatWithDirty("casterStat.absoluteLuck");
        if (absoluteLuck > 0) diceRoll = 6;
        
        ApplyDamage(diceRoll);
    }
}