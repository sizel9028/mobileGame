using System.Collections.Generic;
using UnityEngine;

public partial class GimmickManager
{
    private void PlayRageGain(Character character, Gimmick gimmick)
    {
        var turnType = Battle.Instance.turnType;
        //플레이어 차례일때 character가 플레이어인지, 에너미 차례일때 character가 에너미인지 검사
        bool isMyTurn = character.isPlayer == (turnType == TurnType.PlayerTurn);

        if (!isMyTurn) return;

        character.statMultiplier.rage += character.statMultiplier.rageAddFactor;

    }
}
