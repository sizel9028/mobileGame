using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class PlayerData
{
    // TODO 플레이어 데이터 저장

    //재화
    public int hp;
    public int maxHp;
    public int gold;

    //현재 맵
    public MapNode currentMap;
    //현재 카드
    public Deck playerDeck;
}
