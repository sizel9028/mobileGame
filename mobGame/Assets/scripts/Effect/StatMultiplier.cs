using System;
using UnityEngine;

public class StatMultiplier : ICloneable
{
    //(적의 소환시 공격 계수)
    public float outgoingDamageTotal = 1f;
    
    // 받은 데미지 계수 (예: 약화 디버프에 의해 1.3f)
    public float incomingDamage = 1f;

    // 주는 데미지 계수 (곱연산)
    public float outgoingDamageMultiple = 1f;

    // 주는 데미지 합연산 (힘 상태이상)
    public float outgoingDamageAdd = 0f;

    //턴당 피회복+피감소
    public float turnAddHp = 0f;
    public float turnDecreaseHp = 0f;

    //반사 계수
    //받은 피해 비율만큼 반사
    public float reflectDamageRate = 0f;

    //반사 추가
    public float reflectDamageAdd = 0f;


    // 방어력 증가 합연산 (민첩 상태이상)
    public float outgoingShieldAdd = 0f;
    // 방어력 증가 계수 (예: 강화된 방어 상태일 때 1.2f)
    public float outgoingShieldMultiple = 1f;


    // 카드 드로우 증가 계수 (예: 천리안 같은 버프)
    public float cardDraw = 0f;   //아직 미적용 (카드 첫 드로우시 or 카드 드로우하는 카드에서 적용할듯?)


    // 회복 증가량
    public float outgoingHealAdd = 0f;
    // 회복 증가량 곱연산
    public float outgoingHealMultiple = 1f;

    //추가 마나
    public float addMana = 0f;
    //맵기믹으로 얻는 추가마나
    public float addTurnMana = 0f;

    //상태이상(스턴) :: 공격 불가 (+1 스턴)
    public float stun = 0f;

    public float addHp = 0f; //추가 hp
    public float addShield = 0f; //추가 실드

    //행운
    public float absoluteLuck = 0f; //절대 행운 체크
    public float LuckMultipleDamage = 0f;  //행운 동전

    //민첩성(공격 회피)
    public float agility = 0f; //확률 (0.1 :: 10%)

    //마나 획득 양
    public float manaStack = 0f; //죽이면 해당하는 만큼의 마나를 얻음
    



    // 버서커 계수


    // 분노 수치
    public float rage = 0f;

    //분노 추가 계수
    public float rageAddFactor = 0f;


    // 소환사 계수

    //소환물 죽을때 뼈 반환
    public float corpseReturnCount = 0f;
    // (소환수 자푝)
    //자폭 확률 계수
    public float DeathBlastChance = 0f;

    //자폭 데미지 계수
    public float DeathBlastDamageFactor = 1f;

    //시체 갯수 카운트
    public float CorpseCount = 0f;

    // 시체 폭발 계수 증가
    public float CorpseDamageAdd = 0f;

    // 시체 폭발 계수 증가
    public float CorpseDamageMultiple = 1f;

    // 융합 가능 레벨
    public float fusionLevel = 0f;  //초기 융합 가능 레벨
    //융합시 일정 확률 융합체 복제
    public float fusionClone = 0f; //확률적으로 복제 소환()

    //소환물 파괴시 얻는 값
    public float manaGainChanceWithSummon = 0f;  //소환물 파괴시 일정확률 마나얻음
    public float drawChanceWithSummon = 0f;

    public object Clone()
    {
        return this.MemberwiseClone();
    }
}
