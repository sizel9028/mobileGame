using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EnemySystem : Singleton<EnemySystem>
{
    public IEnumerator PlayCard(CardData card)
    {
        Debug.Log($"[EnemyActionSystem] 적이 카드를 사용합니다: {card.nameKey}");

        // 마나 또는 체력 비용 소비
        SpendCost(card);

        CharacterUI caster = CharacterUIManager.Instance.enemyUIs
        .FirstOrDefault(ui => ui != null && ui.character != null);

        if (caster == null)
        {
            Debug.LogWarning("[EnemyActionSystem] 적 캐스터를 찾을 수 없습니다.");
            yield break;
        }

        var target = GetTargetUI(card);

        if (target == null)
        {
            Debug.LogWarning($"[EnemyActionSystem] 대상이 없어 {card.nameKey} 카드를 사용할 수 없습니다.");
            yield break;
        }

        var processor = new CardProcessor();
        yield return processor.ProcessCardWithTarget(card, caster, target);

    }

    private void SpendCost(CardData card)
    {
        if (card.costType == CostType.Mana)
        {
            EnemyManaSystem.Instance.SpendMana(card.cost);
        }
        else if (card.costType == CostType.Hp)
        {
            //제일 체력많은 애를 깎음
            var candidates = CharacterUIManager.Instance.enemyUIs
                .Where(ui => ui?.character != null && ui.character.currentHp > card.cost)
                .OrderByDescending(ui => ui.character.currentHp)
                .ToList();

            if (candidates.Count > 0)
            {
                candidates[0].character.currentHp -= card.cost;
                Debug.Log($"[EnemyActionSystem] 체력 {card.cost} 소모");
            }
        }
    }
    
    // 상대적으로 target을 반환
    private CharacterUI GetTargetUI(CardData card)
    {
        return card.cardTarget switch
        {
            CardTarget.oneEnemy => CharacterUIManager.Instance.playerUIs
                .FirstOrDefault(ui => ui != null && ui.character != null),

            CardTarget.onePlayer => CharacterUIManager.Instance.enemyUIs
                .FirstOrDefault(ui => ui != null && ui.character != null),

            _ => null // allEnemy, allPlayer 등은 내부 처리
        };
    }
}
