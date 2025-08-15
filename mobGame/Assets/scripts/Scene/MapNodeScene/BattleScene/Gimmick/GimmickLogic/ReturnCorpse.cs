using System.Collections.Generic;

public partial class GimmickManager
{
    private void PlayReturnCorpse(Character character, Gimmick gimmick)
    {
        var list = character.isPlayer ? CharacterUIManager.Instance.playerUIs : CharacterUIManager.Instance.enemyUIs;

        if (list.Count == 0 || list[0] == null) return;

        var leaderStat = list[0].character.statMultiplier;

        // 시체 개수 증가
        leaderStat.CorpseCount += gimmick.gimmicCount;

        gimmick.gimmicCount = 0;
    }
}
