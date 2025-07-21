using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class ManaSystem : Singleton<ManaSystem>
{
    [SerializeField] private ManaUI ManaUI;
    private int Max_Mana = 3;
    private int currentMana;

    public void InitManaSystem()
    {
        getMaxMana();
        Refill();
        ManaUI.UpdateManaText(currentMana);
    }

    public void getMaxMana()
    {
        //TODO 최대 마나 얻기
    }

    public bool HasEnoughMana(int mana)
    {
        return currentMana >= mana;
    }

    public void SpendMana(int mana)
    {
        currentMana -= mana;
        currentMana = Mathf.Max(0, currentMana);
        ManaUI.UpdateManaText(currentMana);
    }

    public void Refill()
    {
        currentMana = Max_Mana;
        ManaUI.UpdateManaText(currentMana);
    }

    public int GetCurrentMana() => currentMana;
}
