using UnityEngine;

public partial class CardEffectProcessor
{
    public int ApplyDamage(float damage, bool doMotion = true)
    {
        float stun = StatWithDirty("casterStat.stun", doMotion);

        if (stun > 0) return 0;

        damage += StatWithDirty("casterStat.outgoingDamageAdd", doMotion);
        damage *= StatWithDirty("casterStat.outgoingDamageMultiple", doMotion);
        damage *= StatWithDirty("targetStat.incomingDamage", doMotion);

        int intDamage = Mathf.RoundToInt(damage);
        if (intDamage < 0) return 0;
        
        if (doMotion) AssistDamage(intDamage);

        //반사처리
        if (intDamage > 0)
        {
            float rate = StatWithDirty("targetStat.reflectDamageRate");
            float add = StatWithDirty("targetStat.reflectDamageAdd");

            int reflectDamage = Mathf.RoundToInt(intDamage * rate + add);

            if (reflectDamage > 0 && casterUI != null)
            {
                casterUI.character.TakeDamage(reflectDamage);
                casterUI.Setup();

                casterUI.Damage();
                Debug.Log($"[Reflect] {targetUI.character.characterArtName} → {casterUI.character.characterArtName} {reflectDamage} 반사!");
            }
        }

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
