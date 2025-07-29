using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using System.Collections;

public class CardProcessor
{
    //카드 계수 바꾸는거 인스턴스화
    private CardEffectProcessor effectProcessor = new();
    private CoefficientModifier coefficientModifier = new();

    private CharacterMotionController motionController = new();

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
        //방어코드
        if (casterUI == null)
        {
            return;
        }

        if (card.cardTarget != CardTarget.nop)
        {
            List<CharacterUI> validTargets = targetUIs?.FindAll(t => t != null);
            if (validTargets == null || validTargets.Count == 0)
            {
                return;
            }
            targetUIs = validTargets;
        }


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
    public IEnumerator ProcessCardWithTarget(CardData card, CharacterUI casterUI, CharacterUI targetUI)
    {
        Battle.Instance.isProcessingCard = true; //카드 실행중
        var casterUIs = GetCasterUIs(card, casterUI);
        var targetUIs = GetTargetUIs(card, casterUI, targetUI);

        if (card.effectMap.ContainsKey("Damage"))
        {
            yield return CharacterUIManager.Instance.StartCoroutine(ProcessCardSequentially(card, casterUIs, targetUIs));
        }
        else
        {
            foreach (var caster in casterUIs)
            {
                if (caster == null || caster.character == null) continue;
                ProcessCard(card, caster, targetUIs);
            }
        }

        //TODO 여기에 기믹카드 추가 
        ProcessGimmick();

        // 게임이 끝났는지 확인
        BattleResult result = CharacterUIManager.Instance.CheckCharacter();

        if (result != BattleResult.Nop)
        {
            Battle.Instance.StartCoroutine(Battle.Instance.EndGame(result));
        }

        Battle.Instance.isProcessingCard = false;
    }


    private IEnumerator ProcessCardSequentially(CardData card, List<CharacterUI> casters, List<CharacterUI> targets, float delay = 0.3f)
    {
        foreach (var caster in casters)
        {
            if (caster == null || caster.character == null) continue;

            yield return motionController.AttackRoutine(caster);
            ProcessCard(card, caster, targets); // 모션 후 처리
            yield return new WaitForSeconds(delay);

        }
    }

    private List<CharacterUI> GetCasterUIs(CardData card, CharacterUI casterUI)
    {
        if (!card.effectMap.ContainsKey("Damage")) return new List<CharacterUI> { casterUI };

        return casterUI.isPlayer
            ? CharacterUIManager.Instance.playerUIs.Where(ui => ui != null && ui.character != null).ToList()
            : CharacterUIManager.Instance.enemyUIs.Where(ui => ui != null && ui.character != null).ToList();
    }

    private List<CharacterUI> GetTargetUIs(CardData card, CharacterUI casterUI, CharacterUI singleTarget)
    {
        List<CharacterUI> targets;

        switch (card.cardTarget)
        {
            case CardTarget.oneEnemy:
            case CardTarget.onePlayer:
            case CardTarget.nop:
                targets = new List<CharacterUI> { singleTarget };
                break;

            case CardTarget.allEnemy:
                targets = casterUI.isPlayer
                    ? CharacterUIManager.Instance.enemyUIs
                    : CharacterUIManager.Instance.playerUIs;
                break;

            case CardTarget.allPlayer:
                targets = casterUI.isPlayer
                    ? CharacterUIManager.Instance.playerUIs
                    : CharacterUIManager.Instance.enemyUIs;
                break;

            default:
                targets = new List<CharacterUI>();
                break;
        }

        return targets;
    }


    private void ProcessGimmick()
    {
        var allUIs = new List<CharacterUI>();
        allUIs.AddRange(CharacterUIManager.Instance.playerUIs);
        allUIs.AddRange(CharacterUIManager.Instance.enemyUIs);

        foreach (var ui in allUIs)
        {
            if (ui?.character == null) continue;

            foreach (var gimmick in ui.character.gimmicks.ToList()) // 복사본 순회
            {
                GimmickManager.Instance.PlayGimmick(ui.character, gimmick);
            }

            GimmickManager.Instance.ClearGimmick(ui.character);
        }
    }

    //플레이어 코스트만 사용
    public void SpendCost(CardData card, CharacterUI ui, HandView handView)
    {
        Debug.Log("실행");
        switch (card.costType)
        {
            case CostType.Mana:
                var chEffectManager = CharacterUIManager.Instance.playerUIs[0].character.effectCardManager;
                if (chEffectManager != null)
                {
                    chEffectManager.dirtyFlag.Add("addMana");
                }
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
