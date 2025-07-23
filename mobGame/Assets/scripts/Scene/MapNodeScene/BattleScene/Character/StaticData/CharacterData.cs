using System;
using UnityEngine;

//초기 전투씬에 들어갈때 프리팹 생성 정보(소환할 때 한번만 사용) (정적정보만 저장)
public class CharacterData
{
    public string name;   // 캐릭터 이름(UI랑 연결되므로 작성할때 다르면 안됨!)

    public int maxHp;
    public int hp;
    public int baseShield;

}
