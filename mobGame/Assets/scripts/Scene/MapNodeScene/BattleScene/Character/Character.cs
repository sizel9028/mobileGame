using UnityEngine;

public class Character
{
    public int maxHp;
    public int currentHp;
    public int shield;


    // 스탯과 이펙트 카드 매니저를 따로 들고있음
    public StatMultiplier statMultiplier = new();
    public EffectCardManager effectCardManager = new();

    public Character()
    {
        effectCardManager.Setup(statMultiplier);
    }
}
