using UnityEngine;


// 동적 정보를 저장하는 클래스
// 데이터 값을 변경 > UI에 변동된 값을 넘김
public class Character
{

    //정보
    public int maxHp;
    public int currentHp;
    public int shield;

    //덱(에너미만 있음)
    public Deck deck;
    public bool isPlayer;

    public Sprite characterArt;


    // 스탯과 이펙트 카드 매니저를 따로 들고있음
    public StatMultiplier statMultiplier = new();
    public EffectCardManager effectCardManager = new();

    public Character()
    {
        effectCardManager.Setup(statMultiplier);
    }

    public void Setup(CharacterData data)
    {
        maxHp = data.maxHp;
        currentHp = data.hp;
        shield = data.baseShield;
        characterArt = data.characterArt;
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

}
