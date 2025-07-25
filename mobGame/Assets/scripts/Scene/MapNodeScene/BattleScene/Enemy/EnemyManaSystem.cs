using UnityEngine;


// 카드 경우의 수 계산을 위한 가상의 에너미 마나 시스템
public class EnemyManaSystem : Singleton<EnemyManaSystem>
{
    public int currentMana { get; private set; }
    public int maxMana {get; private set;}

    public void InitMana(int max = 3)
    {
        maxMana = max;
        refillMana();
    }

    public void refillMana()
    {
        currentMana = maxMana;
    }

    public bool HasEnoughMana(int amount)
    {
        return currentMana >= amount;
    }

    public void SpendMana(int amount)
    {
        currentMana -= amount;
    }
}
