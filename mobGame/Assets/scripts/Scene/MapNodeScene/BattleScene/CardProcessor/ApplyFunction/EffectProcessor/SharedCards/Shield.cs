using UnityEngine;

public partial class CardEffectProcessor
{
    public int ApplyShield(float shieldAmount, bool doMotion = true)
    {
        shieldAmount += StatWithDirty("casterStat.outgoingShieldAdd", doMotion);
        shieldAmount *= StatWithDirty("casterStat.outgoingShieldMultiple", doMotion);

        int intShield = Mathf.RoundToInt(shieldAmount);

        if (doMotion)
        {
            AssistShield(intShield);
        }

        return intShield;
    }

    private void AssistShield(int intShield)
    {
        if (targetUI != null)
        {
            targetUI.character.shield += intShield;
            targetUI.Setup();

            //이펙트 넣는 자리
        }
    }
}
