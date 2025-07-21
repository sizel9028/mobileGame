using UnityEngine;

// 화살표가 가르키는 대상이 valid한 대상인지 체크
public class TargetingValidator 
{
    private CardData data;

    // 패에서 선택된 카드가 선택가능한지
    public void SetSelectCard(CardUI cardUI)
    {
        data = cardUI.data;
    }

    //타게팅 가능한지 반환하는 함수
    public bool isTargetAble(CharacterUI chUI)
    {
        if (data == null || chUI == null) return false;
        
        if (data.cardTarget == CardTarget.oneEnemy && !chUI.isPlayer)
            return true;

        if (data.cardTarget == CardTarget.onePlayer && chUI.isPlayer)
            return true;

        return false;
    }
}
