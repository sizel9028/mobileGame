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
        //마나 초기화
        if (PassiveProcessor.Instance.playerCh == null)
        {
            Max_Mana = 3;
        }
        else
        {
            Max_Mana += Mathf.RoundToInt(PassiveProcessor.Instance.playerCh.statMultiplier.addMana);
        }
        currentMana = Max_Mana;
        ManaUI.UpdateManaText(currentMana);
    }

    public void getMaxMana()
    {
        //TODO 최대 마나 얻기
        var chUI = CharacterUIManager.Instance.playerUIs[0];
        if (chUI == null)
        {
            Max_Mana = 3; return;
        }

        float addMana = chUI.character.statMultiplier.addMana; //캐릭터 ui에서 들고옴
        chUI.character.effectCardManager.dirtyFlag.Add("addMana");
        float addTurnMana = chUI.character.statMultiplier.addTurnMana;
        int totalMana = 3 + Mathf.RoundToInt(addMana) + Mathf.RoundToInt(addTurnMana);
        
        Max_Mana = totalMana;
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
        getMaxMana();
        currentMana = Max_Mana;
        ManaUI.UpdateManaText(currentMana);
    }

    public void Fill(int amount)
    {
        currentMana += amount;
        currentMana = Mathf.Min(currentMana, Max_Mana);
        ManaUI.UpdateManaText(currentMana);
    }

    public int GetCurrentMana() => currentMana;
}
