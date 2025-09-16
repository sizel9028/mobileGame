using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;

public enum BattleResult
{
    Nop, PlayerWin, EnemyWin
}

public class CharacterUIManager : Singleton<CharacterUIManager>
{
    [Header("생성 위치 정보 저장")]
    [SerializeField] private List<RectTransform> playerSlots;
    [SerializeField] private List<RectTransform> enemySlots;

    [Header("UI가 생성될 부모 오브젝트")]
    [SerializeField] private Transform playerRoot;
    [SerializeField] private Transform enemyRoot;

    [Header("캐릭터 UI 정보 저장")]
    public List<CharacterUI> playerUIs = new();
    public List<CharacterUI> enemyUIs = new();

    [Header("캐릭터 prefab")]
    [SerializeField] private CharacterUI characterUIPrefab;

    private CharacterMotionController motionController = new();  //모션 컨트롤러

    //2 1 0순으로 채움
    public void AddCharacter(Character character)
    {
        var baseCh = character.isPlayer ? PassiveProcessor.Instance.playerCh : PassiveProcessor.Instance.enemyCh;

        //난이도 적용
        MonsterStatScaler.ApplyDifficultyScaling(character);
        character.currentHp += (int)baseCh.statMultiplier.addHp;
        character.maxHp += (int)baseCh.statMultiplier.addHp;

        character.shield += (int)baseCh.statMultiplier.addShield;

        if (character.isPlayer)
        {
            EnsureListSize(playerUIs, playerSlots.Count);

            for (int i = 0; i < playerUIs.Count; i++)
            {
                if (playerUIs[i] == null)
                {
                    int slotIndex = playerSlots.Count - 1 - i; // 역방향 매핑
                    var ui = CreateCharacter(character, playerSlots[slotIndex], playerRoot);
                    playerUIs[i] = ui;
                    return;
                }
            }

            Debug.LogWarning("[CharacterUIManager] 플레이어 슬롯 부족");
        }
        else
        {
            EnsureListSize(enemyUIs, enemySlots.Count);

            for (int i = 0; i < enemyUIs.Count; i++)
            {
                if (enemyUIs[i] == null)
                {
                    int slotIndex = enemySlots.Count - 1 - i;
                    var ui = CreateCharacter(character, enemySlots[slotIndex], enemyRoot);
                    enemyUIs[i] = ui;
                    return;
                }
            }

            Debug.LogWarning("[CharacterUIManager] 적 슬롯 부족");
        }
    }

    private void EnsureListSize(List<CharacterUI> list, int size)
    {
        while (list.Count < size)
        {
            list.Add(null);
        }
    }


    public CharacterUI CreateCharacter(Character character, RectTransform slot, Transform root)
    {
        CharacterUI ui = Instantiate(characterUIPrefab, root);

        RectTransform uiRect = ui.GetComponent<RectTransform>();
        if (uiRect != null)
        {
            uiRect.anchoredPosition = slot.anchoredPosition;
        }

        ui.Setup(character);  //정보 세팅

        CharacterUIManager.Instance.StartCoroutine(motionController.SpawnRoutine(ui));
        return ui;
    }

    public void AddCharacterByName(string name, bool isPlayer)
    {
        CharacterData data = ChdataGenerator.GetData(name);
        if (data == null)
        {
            Debug.LogWarning($"[CharacterUIManager] '{name}' 에 해당하는 캐릭터 데이터를 찾을 수 없습니다.");
            return;
        }

        //현재 캐릭터 정보 세팅
        Character character = new Character();
        character.Setup(data);
        character.isPlayer = isPlayer;

        //TODO기믹추가
        character.gimmicks = GimmickLoader.GetGimmickByName(character.characterArtName);

        Character baseCh;
        if (isPlayer)
        {
            baseCh = PassiveProcessor.Instance.playerCh;
        }
        else
        {
            baseCh = PassiveProcessor.Instance.enemyCh;
        }

        //stat effectCard를 복제한걸 넘김
        if (baseCh.statMultiplier != null)
        {
            character.statMultiplier = (StatMultiplier)baseCh.statMultiplier.Clone();

            //몬스터 시작 공격력 계수에 비례 (플레이어는 영향없음)
            character.statMultiplier.outgoingDamageTotal = data.atkCoef;
        }
        if (baseCh.effectCardManager != null)
        {
            character.effectCardManager = (EffectCardManager)baseCh.effectCardManager.Clone();
        }

        //현재 캐릭터랑 effectCardManager을 연결시킴
        character.effectCardManager.SetupCh(character);

        AddCharacter(character);
    }

    public void AddCharacterByData(CharacterData data, bool isPlayer = true)
    {
        if (data == null)
        {
            Debug.LogWarning("[CharacterUIManager] 전달된 CharacterData가 null입니다.");
            return;
        }

        Character character = new Character();
        character.Setup(data);
        character.isPlayer = isPlayer;

        character.gimmicks = GimmickLoader.GetGimmickByName(data.name);

        Character baseCh = isPlayer ?
            PassiveProcessor.Instance.playerCh :
            PassiveProcessor.Instance.enemyCh;

        if (baseCh.statMultiplier != null)
        {
            character.statMultiplier = (StatMultiplier)baseCh.statMultiplier.Clone();
        }
        if (baseCh.effectCardManager != null)
        {
            character.effectCardManager = (EffectCardManager)baseCh.effectCardManager.Clone();
        }

        character.effectCardManager.SetupCh(character);

        AddCharacter(character);
    }

    //캐릭터를 입력받으면 그에 해당하는 CharacterUI를 반환함
    public CharacterUI GetUI(Character character)
    {
        foreach (var ui in playerUIs)
        {
            if (ui == null) continue;

            if (ui.character == character)
                return ui;
        }

        foreach (var ui in enemyUIs)
        {
            if (ui == null) continue;
            if (ui.character == character)
                return ui;
        }

        Debug.LogWarning("[CharacterUIManager] 대상 캐릭터의 UI를 찾을 수 없음");
        return null;
    }


    //카드 한장을 내면 매번 캐릭터를 체크함과 동시에 게임이 끝났는지도 체크함
    public BattleResult CheckCharacter()
    {

        for (int i = 0; i < playerUIs.Count; i++)
        {
            var ui = playerUIs[i];
            if (ui != null && ui.character.currentHp <= 0)
            {
                ui.DestroySelf();
                playerUIs[i] = null; // null로 비워서 슬롯 유지
            }
        }

        for (int i = 0; i < enemyUIs.Count; i++)
        {
            var ui = enemyUIs[i];
            if (ui != null && ui.character.currentHp <= 0)
            {
                ui.DestroySelf();
                enemyUIs[i] = null;
            }
        }

        if (playerUIs.Count > 0 && playerUIs[0] == null)
        {
            return BattleResult.EnemyWin;
        }

        bool allPlayersDead = playerUIs.All(ui => ui == null);
        bool allEnemiesDead = enemyUIs.All(ui => ui == null);

        if (allPlayersDead)
        {
            return BattleResult.EnemyWin;
        }
        else if (allEnemiesDead)
        {
            return BattleResult.PlayerWin;
        }
        else
        {
            return BattleResult.Nop;
        }

    }


}
