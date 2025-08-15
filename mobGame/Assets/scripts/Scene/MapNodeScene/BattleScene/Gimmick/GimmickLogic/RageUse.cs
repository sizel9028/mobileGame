
using System.Collections.Generic;

public partial class GimmickManager
{
    private void PlayRageUse(Character character, Gimmick gimmick)
    {
        if (character.statMultiplier.rage < gimmick.gimmicCondition) return;

        character.statMultiplier.rage = 0f; //분노 초기화
        var value = gimmick.gimmicCount;

        UnityEngine.Debug.Log("분노 사용");

        //기믹 효과를 카드로 추가
        CardData rageCard = new CardData
        {
            nameKey = "card_rage", // 로컬라이즈 키
            descriptionKey = "card_rage_desc",
            path = "Cards/Rage",   // 리소스 경로 (필요에 맞게)
            cardArtName = "rage_art", // 아트 이름
            cardType = CardType.Action,
            actionType = ActionType.Power,
            cardTarget = CardTarget.onePlayer,
            rare = Rare.TierRage,
            cost = 0,
            costType = CostType.Mana,
            maxTurn = 1,
            maxCount = 0,
            effectMapRaw = $"Rage::{value}" // 문자열로도 저장
        };

        rageCard.effectMap = new Dictionary<string, float>();
        rageCard.effectMap["Rage"] = value;

        Character caster = character;

        // target = 플레이어 자신 (onePlayer니까 자기 자신 리스트로)
        List<Character> targets = new List<Character> { character };

        modifier.ProcessCard(rageCard, caster, targets); 
    }
}
