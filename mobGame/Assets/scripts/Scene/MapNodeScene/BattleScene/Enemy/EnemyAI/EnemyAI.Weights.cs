using System.Collections.Generic;

public partial class EnemyAI
{
    private void AdjustWeights(Dictionary<EnemyPlayType, float> weightsDic)
    {
        AdjustWithEnemyHp(weightsDic);
        AdjustWithPlayerHp(weightsDic);
        AdjustWithTurn(weightsDic);
        AdjustWithSummon(weightsDic);
    }

    private void AdjustWithEnemyHp(Dictionary<EnemyPlayType, float> weightsDic)
    {
        var enemyUIs = CharacterUIManager.Instance.enemyUIs;

        int totalCurrentHp = 0;
        int totalMaxHp = 0;

        foreach (var ui in enemyUIs)
        {
            if (ui == null || ui.character == null) continue;

            totalCurrentHp += ui.character.currentHp + ui.character.shield;
            totalMaxHp += ui.character.maxHp;
        }

        if (totalMaxHp == 0) return; // 방어 코드

        float hpRatio = (float)totalCurrentHp / totalMaxHp;
        float factor = 1f / hpRatio;

        MultiplyWeight(weightsDic, EnemyPlayType.Heal, factor);
        MultiplyWeight(weightsDic, EnemyPlayType.Shield, factor);
        MultiplyWeight(weightsDic, EnemyPlayType.DebuffEnemy, factor * 0.8f);
        MultiplyWeight(weightsDic, EnemyPlayType.Nop, factor * 0.4f);
    }

    // 플레이어 Hp에 따라서 계수 변경
    private void AdjustWithPlayerHp(Dictionary<EnemyPlayType, float> weightsDic)
    {
        var playerUIs = CharacterUIManager.Instance.playerUIs;

        int totalCurrentHp = 0;
        int totalMaxHp = 0;

        foreach (var ui in playerUIs)
        {
            if (ui == null || ui.character == null) continue;

            totalCurrentHp += ui.character.currentHp + ui.character.shield;
            totalMaxHp += ui.character.maxHp;
        }

        if (totalMaxHp == 0 || totalCurrentHp == 0) return;

        float hpRatio = (float)totalCurrentHp / totalMaxHp;
        float factor = 1f / hpRatio;

        MultiplyWeight(weightsDic, EnemyPlayType.Attack, factor * 0.3f);
    }

    //턴 기반 가중치 두기
    private void AdjustWithTurn(Dictionary<EnemyPlayType, float> weightsDic)
    {
        int turn = Battle.Instance.turnCount;

        if (turn <= 3)
        {
            MultiplyWeight(weightsDic, EnemyPlayType.BuffSelf, 3f);
            MultiplyWeight(weightsDic, EnemyPlayType.Shield, 1.8f);
        }
        else if (turn <= 6)
        {
            MultiplyWeight(weightsDic, EnemyPlayType.Attack, 1.3f);
            MultiplyWeight(weightsDic, EnemyPlayType.DebuffEnemy, 1.3f);
        }
        else
        {
            MultiplyWeight(weightsDic, EnemyPlayType.Heal, 1.5f);
            MultiplyWeight(weightsDic, EnemyPlayType.Shield, 1.5f);
            MultiplyWeight(weightsDic, EnemyPlayType.DebuffEnemy, 1.3f);
        }
    }

    private void AdjustWithSummon(Dictionary<EnemyPlayType, float> weightsDic)
    {
        MultiplyWeight(weightsDic, EnemyPlayType.Summon, 7f);
    }

    
    //해당하는 계수에 factor만큼 곱함
    private void MultiplyWeight(Dictionary<EnemyPlayType, float> dic, EnemyPlayType type, float factor)
    {
        if (dic.ContainsKey(type))
        {
            dic[type] *= factor;
        }
    }

}