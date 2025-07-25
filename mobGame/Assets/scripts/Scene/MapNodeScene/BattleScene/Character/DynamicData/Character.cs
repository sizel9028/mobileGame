using System;
using UnityEngine;


// 동적 정보를 저장하는 클래스
// 데이터 값을 변경 > UI에 변동된 값을 넘김
public class Character : ICloneable
{

    //정보
    public string characterArtName;
    public int maxHp;
    public int currentHp;
    public int shield;

    //덱(에너미만 있음)
    public bool isPlayer;


    // 스탯과 이펙트 카드 매니저를 따로 들고있음
    public StatMultiplier statMultiplier = new();
    public EffectCardManager effectCardManager = new();


    public Character()
    {
        effectCardManager.Setup(statMultiplier);
    }

    public void Setup(CharacterData data)
    {
        characterArtName = data.name;
        maxHp = data.maxHp;
        currentHp = data.hp;
        shield = data.baseShield;
    }

    //내부 값을 바꾸는 함수(데미지, 힐같은 함수)
    public void TakeDamage(int amount)
    {
        int damageAfterShield = Mathf.Max(0, amount - shield);
        shield = Mathf.Max(0, shield - amount);
        currentHp = Mathf.Max(0, currentHp - damageAfterShield);

        Debug.Log($"[Character] 데미지 {amount} 적용됨 (실피해: {damageAfterShield}), 남은 HP: {currentHp}, 남은 쉴드: {shield}");
    }

    public void Heal(int amount)
    {
        currentHp = Mathf.Min(currentHp + amount, maxHp);
        Debug.Log($"[Character] 힐 {amount} 적용됨, 현재 HP: {currentHp}");
    }

    public object Clone()
    {
        var clone = new Character();

        //클래스 내부 값 복사
        clone.characterArtName = this.characterArtName;
        clone.maxHp = this.maxHp;
        clone.currentHp = this.currentHp;
        clone.shield = this.shield;
        clone.isPlayer = this.isPlayer;

        // 스탯과 효과 매니저도 복사
        clone.statMultiplier = (StatMultiplier)this.statMultiplier.Clone();
        clone.effectCardManager = (EffectCardManager)this.effectCardManager.Clone();

        //effectCardManager 세팅
        clone.effectCardManager.SetupCh(clone);
        clone.effectCardManager.Setup(clone.statMultiplier);

        return clone;
    }

}
