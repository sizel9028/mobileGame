using UnityEngine;

public partial class CoefficientModifier
{
    /// <summary>
    /// fusionLevel을 적용한다. 여러 개가 있어도 가장 높은 값만 적용되도록 한다.
    /// </summary>
    private void ApplyFusionLevel(float amount, Character caster, Character target)
    {
        // caster의 fusionLevel과 새로 들어온 값 중 더 높은 값만 유지
        if (amount > caster.statMultiplier.fusionLevel)
        {
            caster.statMultiplier.fusionLevel = amount;
        }

    }

}
