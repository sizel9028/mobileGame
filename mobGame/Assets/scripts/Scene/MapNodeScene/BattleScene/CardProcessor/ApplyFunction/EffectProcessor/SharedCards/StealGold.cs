using System.Collections;
using UnityEngine;

public partial class CardEffectProcessor
{

    //적이 플레이어의 골드를 약탈함
    private void ApplyStealGold(float amount)
    {
        GameManager.gameManager.playerData.gold -= (int)amount;
    }
}