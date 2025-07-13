using UnityEngine;

public class StatMultiplier
{
    // 받은 데미지 계수 (예: 약화 디버프에 의해 1.3f)
    public float incomingDamage = 1f; 
    // 주는 데미지 계수 (곱연산)
    public float outgoingDamage = 1f; 
    // 주는 데미지 합연산 (힘 상태이상)
    
    // 방어력 증가 합연산 (민첩 상태이상)
    
    // 방어력 증가 계수 (예: 강화된 방어 상태일 때 1.2f)
    public float blockGain = 1f; 
    // 받는 방어력 감소 계수 (예: debuff로 방어량 -50%일 때 0.5f)
    public float blockReceived = 1f;
    // 카드 드로우 증가 계수 (예: 천리안 같은 버프)
    public float cardDraw = 1f;
    // 카드 코스트 절감 계수 (예: 일시적인 마나 절감 효과)
    public float manaCostReduction = 1f;
    // 상태이상 지속 시간 계수 (예: 상태이상 저항이 있으면 0.8f)
    public float debuffDuration = 1f;
    // 힐 계수 (받는 회복량 증감)
    public float healReceive = 1f;
    // bool 스턴 유무 T/F
    
    // 흡혈 계수 (예: Lifesteal 상태일 때 피해량의 50%를 체력 회복)
    public float lifesteal = 0f;
}
