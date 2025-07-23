using UnityEngine;

public partial class CardEffectProcessor
{
    public int ApplyHeal(float amount, bool doMotion = true)
    {
        amount += StatWithDirty("casterStat.outgoingHealAdd", doMotion);
        amount *= StatWithDirty("casterStat.outgoingHealMultiple", doMotion);

        int intHeal = Mathf.RoundToInt(amount);

        if (targetUI != null)
        {
            int possibleHeal = targetUI.character.maxHp - targetUI.character.currentHp;
            intHeal = Mathf.Clamp(intHeal, 0, possibleHeal);

            if (doMotion)
            {
                AssistHeal(intHeal);
            }

            return intHeal;
        }

        return 0;
    }

    private void AssistHeal(int intHeal)
    {
        if (targetUI != null && intHeal > 0)
        {
            targetUI.character.currentHp += intHeal;
            targetUI.Setup();

            // 힐 이펙트 넣는 자리

            Debug.Log($"[CardEffectProcessor] 힐 적용됨: +{intHeal} HP");
        }
    }
}