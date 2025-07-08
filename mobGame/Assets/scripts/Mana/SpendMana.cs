using UnityEngine;

public class SpendMana : GameAction
{
    public int Amount { get; set; }
    public SpendMana(int amount)
    {
        Amount = amount;
    }
}
