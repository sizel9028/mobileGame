using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class CardEffectProcessor
{
    private void ApplyFusion()
    {
        var playerUIs = CharacterUIManager.Instance.playerUIs;

        if (playerUIs.Count < 3 || playerUIs[1] == null || playerUIs[2] == null || playerUIs[0] == null)
        {
            Debug.LogWarning("[Fusion] 융합할 몬스터가 부족합니다.");
            return;
        }

        int level = (int)playerUIs[0].character.statMultiplier.fusionLevel;
        string name1 = playerUIs[1].character.characterArtName;
        string name2 = playerUIs[2].character.characterArtName;

        string fusion = SummonDataLoader.GetFusionResult(name1, name2, level);
        if (fusion == null) return;

        playerUIs[1].DestroySelf(); playerUIs[2].DestroySelf();

        Battle.Instance.StartCoroutine(SpawnFusionCh(fusion));

        float cloneChance = playerUIs[0].character.statMultiplier.fusionClone;
        if (Random.value < cloneChance)
        {
            Battle.Instance.StartCoroutine(SpawnFusionCh(fusion));
        }
    }

    private IEnumerator SpawnFusionCh(string fusion)
    {
        yield return new WaitForSeconds(0.5f);
        CharacterUIManager.Instance.AddCharacterByName(fusion, isPlayer: true);

        var playerUIs = CharacterUIManager.Instance.playerUIs;
        if (playerUIs[2] != null)
        {
            playerUIs[2].character.maxHp = 1;
            playerUIs[2].character.currentHp = 1;
            playerUIs[2].Setup();
        }
    }
}