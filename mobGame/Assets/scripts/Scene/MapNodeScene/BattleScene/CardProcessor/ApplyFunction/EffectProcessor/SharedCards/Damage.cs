using UnityEngine;

public partial class CardEffectProcessor
{
    public int ApplyDamage(float damage, bool doMotion = true)
    {
        float stun = StatWithDirty("casterStat.stun", doMotion);

        if (stun > 0) return 0;

        float luckyCoinCount = StatWithDirty("casterStat.LuckMultipleDamage");
        if (luckyCoinCount < 0) return 0;

        damage += StatWithDirty("casterStat.outgoingDamageAdd", doMotion);
        damage *= StatWithDirty("casterStat.outgoingDamageMultiple", doMotion);
        damage *= StatWithDirty("targetStat.incomingDamage", doMotion);
        damage *= StatWithDirty("casterStat.outgoingDamageTotal");  // 몬스터 전용
        //Debug.Log($"[CharacterUIManager]  outgoingDamageTotal = {casterUI.character.statMultiplier.outgoingDamageTotal}");
        damage *= Mathf.Pow(2f, luckyCoinCount);

        int intDamage = Mathf.RoundToInt(damage);
        if (intDamage < 0) return 0;

        if (CheckEvasion())
        {
            Debug.Log($"[Evasion] {targetUI.character.characterArtName} 공격 회피!");
            //miss모션을 처리
            DamageUI.Instance.ShowDamage(targetUI, 0, true);
            return 0;
        }

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

                DamageUI.Instance.ShowDamage(casterUI, reflectDamage, false);
            }
        }

        if (intDamage > 0)
        {
            DamageUI.Instance.ShowDamage(targetUI, intDamage, false);
            //SoundManager.Instance.GetHitSound();
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

    //민첩인지 체크
    private bool CheckEvasion()
    {
        float agility = StatWithDirty("targetStat.agility");
        return Random.value < agility;
    }
}
