using UnityEngine;

public partial class CoefficientModifier
{
    private void ApplyLuckyCoin(float amount, Character caster, Character target)
    {
        float roll = Random.value; // 랜덤값 확인용
        bool success = roll < 0.7f;
        if (caster.statMultiplier.absoluteLuck > 0) success = true;
        caster.effectCardManager.dirtyFlag.Add("absoluteLuck");  //절대 행운 제한

        var casterUI = CharacterUIManager.Instance.GetUI(caster);
        if (casterUI == null) return;

        if (amount < 0)
        {
            caster.statMultiplier.LuckMultipleDamage = 0f; //초기화
            casterUI.SetLuckyCoinOutline(0);
            return;
        }

        if (caster.statMultiplier.LuckMultipleDamage < 0)
        {
            casterUI.SetLuckyCoinOutline(-1);
            return; //0이니깐 리턴시킴
        }

        Debug.Log($"[LuckyCoin] SUCCESS! roll={roll:F2}, amount={amount}, total={caster.statMultiplier.LuckMultipleDamage}");

        if (success)
        {
            caster.statMultiplier.LuckMultipleDamage += amount;
            casterUI.SetLuckyCoinOutline(1);
        }
        else
        {
            caster.statMultiplier.LuckMultipleDamage = -1f;
            casterUI.SetLuckyCoinOutline(-1);
        }
        
    }

}
