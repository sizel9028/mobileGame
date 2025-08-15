using System;
using UnityEngine;

public class StatMultiplier : ICloneable
{
    public float outgoingDamageTotal = 1f;
    // 받은 데미지 계수 (예: 약화 디버프에 의해 1.3f)
    public float incomingDamage = 1f;
    // 주는 데미지 계수 (곱연산)
    public float outgoingDamageMultiple = 1f;
    // 주는 데미지 합연산 (힘 상태이상)
    public float outgoingDamageAdd = 0f;


    // 방어력 증가 합연산 (민첩 상태이상)
    public float outgoingShieldAdd = 0f;
    // 방어력 증가 계수 (예: 강화된 방어 상태일 때 1.2f)
    public float outgoingShieldMultiple = 1f;


    // 카드 드로우 증가 계수 (예: 천리안 같은 버프)
    public float cardDraw = 1f;


    // 회복 증가량
    public float outgoingHealAdd = 0f;
    // 회복 증가량 곱연산
    public float outgoingHealMultiple = 1f;

    //추가 마나
    public float addMana = 0f;


    // 카드 코스트 절감 계수 (예: 일시적인 마나 절감 효과)
    public float manaCostReduction = 1f;
    // 상태이상 지속 시간 계수 (예: 상태이상 저항이 있으면 0.8f)
    public float debuffDuration = 1f;
    // 힐 계수 (받는 회복량 증감)
    public float healReceive = 1f;
    // bool 스턴 유무 T/F

    // 흡혈 계수 (예: Lifesteal 상태일 때 피해량의 50%를 체력 회복)
    public float lifesteal = 0f;

    // 버서커 계수


    // 분노 수치
    public float rage = 0f;

    //분노 추가 계수
    public float rageAddFactor = 1f;


    // 소환사 계수

    // (소환수 자푝)
    //자폭 확률 계수
    public float DeathBlastChance = 1f;

    //자폭 데미지 계수
    public float DeathBlastDamageFactor = 1f;

    //시체 갯수 카운트
    public float CorpseCount = 6f;

    // 시체 폭발 계수 증가
    public float CorpseDamageAdd = 0f;

    // 시체 폭발 계수 증가
    public float CorpseDamageMultiple = 1f;




    public object Clone()
    {
        return this.MemberwiseClone();
    }
}
