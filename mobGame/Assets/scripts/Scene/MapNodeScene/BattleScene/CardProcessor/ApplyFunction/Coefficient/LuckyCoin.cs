using UnityEngine;

public partial class CoefficientModifier
{
    private void ApplyLuckyCoin(float amount, Character caster, Character target)
    {
        float roll = Random.value; // 랜덤값 확인용
        bool success = roll < 0.5f;
        if (target.statMultiplier.absoluteLuck > 0) success = true;
        target.effectCardManager.dirtyFlag.Add("absoluteLuck");  //절대 행운 제한

        var targetUI = CharacterUIManager.Instance.GetUI(target);
        if (targetUI == null) return;

        if (amount < 0)
        {
            target.statMultiplier.LuckMultipleDamage = 0f; //초기화
            targetUI.SetLuckyCoinOutline(0);
            return;
        }

        if (target.statMultiplier.LuckMultipleDamage < 0)
        {
            targetUI.SetLuckyCoinOutline(-1);
            return; //0이니깐 리턴시킴
        }

        //Debug.Log($"[LuckyCoin] SUCCESS! roll={roll:F2}, amount={amount}, total={caster.statMultiplier.LuckMultipleDamage}");

        if (success)
        {
            target.statMultiplier.LuckMultipleDamage += amount;
            targetUI.SetLuckyCoinOutline(1);
        }
        else
        {
            target.statMultiplier.LuckMultipleDamage = -1f;
            targetUI.SetLuckyCoinOutline(-1);
        }
        
    }

}
