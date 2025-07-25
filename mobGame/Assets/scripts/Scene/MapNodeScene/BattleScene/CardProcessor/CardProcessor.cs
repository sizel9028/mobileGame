using System.Collections.Generic;
using UnityEngine;

public class CardProcessor
{
    //카드 계수 바꾸는거 인스턴스화
    private CardEffectProcessor effectProcessor = new();
    private CoefficientModifier coefficientModifier = new();

    public void ProcessCard(CardData card, List<CharacterUI> casterUIs, List<CharacterUI> targetUIs)
    {
        foreach (var casterUI in casterUIs)
        {
            ProcessCard(card, casterUI, targetUIs);
        }
    }
    //카드 데이터와 caster, targets을 넣으면 카드를 적용시킴
    public void ProcessCard(CardData card, CharacterUI casterUI, List<CharacterUI> targetUIs)
    {
        switch (card.actionType)
        {
            case ActionType.Skill:
                effectProcessor.ProcessCardEffect(card, casterUI, targetUIs);
                break;

            case ActionType.Power:
                coefficientModifier.ProcessCardEffect(card, casterUI, targetUIs);
                break;
        }
    }

    //target이 하나인 함수
    public void ProcessCard(CardData card, CharacterUI casterUI, CharacterUI targetUI)
    {
        List<CharacterUI> targetUIs = new() { targetUI };
        ProcessCard(card, casterUI, targetUIs);
    }

    //casterUI랑 targetUI를 넣으면 바로 알아서 세팅해줌
    public void ProcessCardWithTarget(CardData card, CharacterUI casterUI, CharacterUI targetUI)
    {
        if (!card.effectMap.ContainsKey("Damage"))
        {
            switch (card.cardTarget)
            {
                case CardTarget.nop:
                case CardTarget.oneEnemy:
                case CardTarget.onePlayer:
                    ProcessCard(card, casterUI, targetUI);
                    break;

                case CardTarget.allEnemy:
                    {
                        var targets = casterUI.isPlayer
                            ? CharacterUIManager.Instance.enemyUIs
                            : CharacterUIManager.Instance.playerUIs;

                        var validTargets = targets.FindAll(t => t != null && t.character != null);
                        ProcessCard(card, casterUI, validTargets);
                        break;
                    }

                case CardTarget.allPlayer:
                    {
                        var targets = casterUI.isPlayer
                            ? CharacterUIManager.Instance.playerUIs
                            : CharacterUIManager.Instance.enemyUIs;

                        var validTargets = targets.FindAll(t => t != null && t.character != null);
                        ProcessCard(card, casterUI, validTargets);
                        break;
                    }
            }
        }
        else
        {
            var allCasters = casterUI.isPlayer
                ? CharacterUIManager.Instance.playerUIs
                : CharacterUIManager.Instance.enemyUIs;

            switch (card.cardTarget)
            {
                case CardTarget.oneEnemy:
                    foreach (var caster in allCasters)
                    {
                        ProcessCard(card, caster, targetUI);
                    }
                    break;

                case CardTarget.allEnemy:
                    {
                        var targets = casterUI.isPlayer
                            ? CharacterUIManager.Instance.enemyUIs
                            : CharacterUIManager.Instance.playerUIs;

                        var validTargets = targets.FindAll(t => t != null && t.character != null);
                        foreach (var caster in allCasters)
                        {
                            ProcessCard(card, caster, validTargets);
                        }
                        break;
                    }

                case CardTarget.onePlayer:
                case CardTarget.allPlayer:
                case CardTarget.nop:
                    // 데미지 카드인데 플레이어 대상이거나 nop? → 그대로 단일 처리
                    ProcessCard(card, casterUI, targetUI);
                    break;
            }
        }
        //TODO 여기에 기믹카드 추가 

        // 게임이 끝났는지 확인
        BattleResult result = CharacterUIManager.Instance.CheckCharacter();

        if (result != BattleResult.Nop)
        {
            Battle.Instance.EndGame();
        }
    }

    //플레이어 코스트만 사용
    public void SpendCost(CardData card, CharacterUI ui, HandView handView)
    {
        Debug.Log("실행");
        switch (card.costType)
        {
            case CostType.Mana:
                ManaSystem.Instance.SpendMana(card.cost);
                Debug.Log($"[코스트] 마나 {card.cost} 소모됨");
                break;

            case CostType.Hp:
                // 플레이어는 항상 playerUIs[2]에 있다고 가정
                var playerUI = CharacterUIManager.Instance.playerUIs.Count > 2
                    ? CharacterUIManager.Instance.playerUIs[2]
                    : null;

                if (playerUI?.character != null)
                {
                    playerUI.character.currentHp -= card.cost;
                    Debug.Log($"[코스트] 체력 {card.cost} 소모됨 → 남은 체력: {playerUI.character.currentHp}");

                    //TODO 체력 UI 갱신
                }
                break;
        }

        handView.CheckUsableCard();
    }

}
