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
                value += 5;
                break;

            case "outgoingDamageAdd":
                value += 2;
                break;

            case "outgoingDamageMultiple":
                value += 0.05f;
                break;

            case "incomingDamage":
                value += 0.02f * Mathf.Sign(value);
                break;

            case "Draw":
                value += 1;
                break;

            case "outgoingShieldAdd":
                value += 2;
                break;

            case "outgoingShieldMultiple":
                value += 0.05f;
                break;

            case "Shield":
                value += 5;
                break;

            case "MaxHPAdd":
                value += 2;
                break;

            case "agility":
                value += 0.05f;
                break;

            case "ddMana":
                value += 1;
                break;

            case "Heal":
                value += 10;
                break;

            case "BoneShield":
                value += 0.05f;
                break;

            case "CorpseExplosion":
                value += 0.01f;
                break;

            case "DeathBlastChance":
                value += 0.05f;
                break;

            case "DeathBlastDamageFactor":
                value += 0.07f;
                break;

            case "manaGainChanceWithSummon":
                value += 0.05f;
                break;

            case "drawChanceWithSummon":
                value += 0.04f;
                break;

            case "corpseReturnCount":
                value += 1;
                break;
        }

        card.effectMap[key] = value;
    }
}
