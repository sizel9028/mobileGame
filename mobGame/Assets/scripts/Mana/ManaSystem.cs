using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class ManaSystem : Singleton<ManaSystem>
{
    [SerializeField] private ManaUI ManaUI;
    private const int Max_Mana = 3;
    private int currentMana = Max_Mana;
    void OnEnable()
    {
        ActionSystem.AttachPerformer<SpendMana>(SpendManaPerformer);
        ActionSystem.AttachPerformer<RefillMana>(RefillManaPerformer);
    }
    void OnDisable()
    {
        ActionSystem.DetachPerformer<SpendMana>();
        ActionSystem.DetachPerformer<RefillMana>();
    }

    public bool HasEnoughMana(int mana)
    {
        return currentMana >= mana;
    }

    private IEnumerator SpendManaPerformer(SpendMana spendMana)
    {
        currentMana -= spendMana.Amount;
        ManaUI.UpdateManaText(currentMana);
        yield return null;
    }

    private IEnumerator RefillManaPerformer(RefillMana refillMana)
    {
        currentMana = Max_Mana;
        ManaUI.UpdateManaText(currentMana);
        yield return null;
    }
}
