using UnityEngine;

public partial class CoefficientModifier
{
    private void ApplyStun(float amount, Character caster, Character target)
    {
        var targetUI = CharacterUIManager.Instance.GetUI(target);
        if (targetUI == null) return;

        target.statMultiplier.stun += amount;

        targetUI.SetGrayState((int)target.statMultiplier.stun);
    }
}
