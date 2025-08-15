using System.Collections.Generic;
using UnityEngine;

public class UpgradeProcessor
{
    public void UpgradeCard(CardData card)
    {
        if (card == null) return;

        var keys = new List<string>(card.effectMap.Keys);

        foreach (var key in keys)
        {
            DoEffect(card, key);
        }
    }

    private void DoEffect(CardData card, string key)
    {
        float value = card.effectMap[key];

        //강화를 통해서 값을 각 역할에 따라 변경함
        switch (key)
        {
            case "Damage":
                value += 50;
                break;
        }

        card.effectMap[key] = value;
    }
}
