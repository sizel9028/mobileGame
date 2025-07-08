using System;
using UnityEngine;

//초기 전투씬에 들어갈때 프리팹 생성 정보(소환할 때 한번만 사용)
public class CharacterData
{
    public string name;   // 몬스터 이름 (혹시 모르니 저장)
    public string path;
    [NonSerialized] public Sprite characterArt;

    public int hp;
    public int baseShield;

    public void LoadArt()
    {
        string fullPath = $"{path}/{"enemyData"}";
        characterArt = Resources.Load<Sprite>(fullPath);
        if (characterArt == null)
        {
            Debug.LogWarning($"[CharacterData] 초상화 로드 실패: {fullPath}");
        }
    }
}
