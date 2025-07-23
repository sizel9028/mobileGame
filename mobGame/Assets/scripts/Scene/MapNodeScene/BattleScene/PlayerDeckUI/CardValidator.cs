using UnityEngine;


//현재 사용 가능한 카드인지 반환
public static class CardValidator
{
    public static bool IsCardAble(CardData data, bool isPlayer)
    {
        // 카드 마나 조건 (체력 조건도 추가)
        switch (data.costType)
        {
            case CostType.Mana:
                if (!ManaSystem.Instance.HasEnoughMana(data.cost))
                    return false;
                break;

            case CostType.Hp:
                int currHp = GetCurrentHp(isPlayer);
                if (currHp <= data.cost) return false;

                break;
        }

        if (data.effectMap.ContainsKey("summons")) //소환 조건
        {
            var list = isPlayer ? CharacterUIManager.Instance.playerUIs : CharacterUIManager.Instance.enemyUIs;

            if (list == null || list.Count == 3) return false;
        }

        return true;
    }

    private static int GetCurrentHp(bool isPlayer)
    {
        if (isPlayer)
        {
            var playerUI = CharacterUIManager.Instance.playerUIs[2];

            return playerUI?.character?.currentHp ?? 0;
        }
        else
        {
            int maxHp = 0;

            foreach (var enemyUI in CharacterUIManager.Instance.enemyUIs)
            {
                if (enemyUI?.character != null)
                {
                    int hp = enemyUI.character.currentHp;
                    if (hp > maxHp) maxHp = hp;
                }
            }

            return maxHp;
        }
    }
}
