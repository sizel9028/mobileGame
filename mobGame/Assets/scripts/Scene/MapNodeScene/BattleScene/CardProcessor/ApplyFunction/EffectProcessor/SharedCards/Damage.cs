using UnityEngine;

public partial class CardEffectProcessor
{
    public int ApplyDamage(float damage, bool doMotion = true)
    {
        damage += StatWithDirty("casterStat.outgoingDamageAdd", doMotion);
        damage *= StatWithDirty("casterStat.outgoingDamageMultiple", doMotion);
        damage *= StatWithDirty("targetStat.incomingDamage", doMotion);

        int intDamage = Mathf.RoundToInt(damage);

        if (doMotion) AssistDamage(intDamage);

        return intDamage;
    }

    private void AssistDamage(int intDamage)
    {
        if (targetUI != null)
        {
            targetUI.character.TakeDamage(intDamage);
            targetUI.Setup();

            if (intDamage > 0)
            {
                targetUI.Damage();
            }
        }
    }
}
