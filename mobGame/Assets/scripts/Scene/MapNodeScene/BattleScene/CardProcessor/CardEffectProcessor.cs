using System.Collections.Generic;
using UnityEngine;

public partial class CardEffectProcessor
{
    // 적용할거 매번 적기 귀찮아서 지역변수로 설정
    private StatMultiplier casterStat;
    private StatMultiplier targetStat;
    private CharacterUI casterUI;
    private CharacterUI targetUI;

    private CoefficientModifier modifier = new();


    public void ProcessCardEffect(CardData card, CharacterUI casterUI, List<CharacterUI> targetUIs)
    {

        foreach (var effectKey in card.effectMap.Keys)
        {
            var effect = card.effectMap[effectKey];

            if (card.cardTarget == CardTarget.oneEnemy || card.cardTarget == CardTarget.onePlayer)
            {
                if (targetUIs.Count > 0)
                {
                    ApplyEffectSingle(effectKey, effect, casterUI, targetUIs[0]);
                }
            }
            else
            {
                ApplyEffectMultiple(effectKey, effect, casterUI, targetUIs);
            }
        }

        //카드 한장이 사용되면 계수카드 쓰인거 삭제
        if (casterUI?.character?.effectCardManager != null)
        {
            casterUI.character.effectCardManager.CheckCount();
        }

        foreach (var targetUI in targetUIs)
        {
            if (targetUI?.character?.effectCardManager != null)
            {
                targetUI.character.effectCardManager.CheckCount();
            }
        }

        casterStat = null; casterUI = null; targetStat = null; targetUI = null;

    }

    //target이 여러명일때
    private void ApplyEffectMultiple(string effectKey, float effect, CharacterUI casterUI, List<CharacterUI> targetUIs)
    {
        foreach (var targetUI in targetUIs)
        {
            ApplyEffectSingle(effectKey, effect, casterUI, targetUI);
        }
    }

    //target이 single일때
    private void ApplyEffectSingle(string effectKey, float effect, CharacterUI casterUI, CharacterUI targetUI)
    {
        this.casterUI = casterUI;
        this.targetUI = targetUI;
        
        if (casterUI?.character != null)
        {
            casterStat = casterUI.character.statMultiplier;
        }
        if (targetUI?.character != null)
        {
            targetStat = targetUI.character.statMultiplier;
        }

        //TODO switch문으로 효과 나누기
        switch (effectKey)
        {
            case "Damage":
                ApplyDamage(effect);
                break;

            case "Shield":
                ApplyShield(effect);
                break;

            case "Draw":
                ApplyDraw(effect);
                break;

            case "Heal":
                ApplyHeal(effect);
                break;

            case "Summon":
                ApplySummon(effect);
                break;

            case "CorpseExplode":
                ApplyCorpseExplode(effect);
                break;

            case "BoneShield":
                ApplyBoneShield(effect);
                break;

            case "Fusion":
                ApplyFusion();
                break;

            case "Sacrifice":
                ApplySacrifice();
                break;

            case "BoneSpear":
                ApplyBoneSpear(effect);
                break;

            case "BoneGolem":
                ApplySummonBoneGolem(effect);
                break;

            case "ClearBuffWithApply":
                ApplyClearBuffWithApply(effect);
                break;

            case "ClearBuffWithoutApply":
                ApplyClearBuffWithoutApply(effect);
                break;

            case "CardCostToInt":
                ApplyCardCostToInt(effect);
                break;

            case "StealGold":
                ApplyStealGold(effect);
                break;

            case "DrawHighTierCard":
                ApplyDrawHighTierCard();
                break;

            case "DiceDamage":
                DiceDamage();
                break;
        }   
    }


    //카드 효과가 발동되면 이 값을 effectCardManager에 넘겨서 턴, 횟수 체크
    public float StatWithDirty(string expr, bool doMotion = true)
    {
        string[] parts = expr.Split('.'); // ex: "casterStat.outgoingDamageAdd"
        if (parts.Length != 2)
        {
            Debug.LogError($"[StatWithDirty] 잘못된 형식: {expr}");
            return 0;
        }

        string source = parts[0];
        string fieldName = parts[1];

        if (source == "casterStat")
        {
            //null을 입력받으면
            if (casterStat == null)
            {
                casterStat = new StatMultiplier();
            }
            // motion을 안하면 값만 단순히 반환시킴
            if (doMotion && casterUI != null) casterUI.character.effectCardManager.dirtyFlag.Add(fieldName);

            var field = casterStat.GetType().GetField(fieldName);
            if (field != null && field.GetValue(casterStat) is float val)
            {
                return val;
            }

            Debug.LogError($"[StatWithDirty] casterStat에 '{fieldName}' 필드가 없거나 float이 아님");
            return 1;
        }
        else if (source == "targetStat")
        {
            if (targetStat == null)
            {
                targetStat = new StatMultiplier();
            }

            if (doMotion && targetUI != null) targetUI.character.effectCardManager.dirtyFlag.Add(fieldName);

            var field = targetStat.GetType().GetField(fieldName);
            if (field != null && field.GetValue(targetStat) is float val)
            {
                return val;
            }

            Debug.LogError($"[StatWithDirty] targetStat에 '{fieldName}' 필드가 없거나 float이 아님");
            return 1;
        }
        else
        {
            Debug.LogError($"[StatWithDirty] 알 수 없는 stat source: {source}");
            return 1;
        }
    }
    
}
