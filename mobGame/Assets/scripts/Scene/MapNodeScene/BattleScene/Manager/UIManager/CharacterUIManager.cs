using System.Collections.Generic;
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

    //2 1 0순으로 채움
    public void AddCharacter(Character character)
    {
        if (character.isPlayer)
        {
            int index = playerSlots.Count - 1 - playerUIs.Count;
            if (index < 0)
            {
                Debug.LogWarning("[CharacterUIManager] 플레이어 슬롯 부족");
                return;
            }

            var ui = CreateCharacter(character, playerSlots[index], playerRoot);
            playerUIs.Add(ui);
        }
        else
        {
            int index = enemySlots.Count - 1 - enemyUIs.Count;
            if (index < 0)
            {
                Debug.LogWarning("[CharacterUIManager] 적 슬롯 부족");
                return;
            }

            var ui = CreateCharacter(character, enemySlots[index], enemyRoot);
            enemyUIs.Add(ui);
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
        }
        if (baseCh.effectCardManager != null)
        {
            character.effectCardManager = (EffectCardManager)baseCh.effectCardManager.Clone();
        }

        //현재 캐릭터랑 effectCardManager을 연결시킴
        character.effectCardManager.SetupCh(character);

        AddCharacter(character);
    }

    //캐릭터를 입력받으면 그에 해당하는 CharacterUI를 반환함
    public CharacterUI GetUI(Character character)
    {
        foreach (var ui in playerUIs)
        {
            if (ui.character == character)
                return ui;
        }

        foreach (var ui in enemyUIs)
        {
            if (ui.character == character)
                return ui;
        }

        Debug.LogWarning("[CharacterUIManager] 대상 캐릭터의 UI를 찾을 수 없음");
        return null;
    }


    //카드 한장을 내면 매번 캐릭터를 체크함과 동시에 게임이 끝났는지도 체크함
    public BattleResult CheckCharacter()
    {

        // 체력 0을 모두 제거시킴
        playerUIs.RemoveAll(ui =>
        {
            if (ui.character.currentHp <= 0)
            {
                ui.DestroySelf();
                return true;
            }
            return false;
        });

        enemyUIs.RemoveAll(ui =>
        {
            if (ui.character.currentHp <= 0)
            {
                ui.DestroySelf();
                return true;
            }
            return false;
        });

        if (playerUIs.Count == 0)
        {
            return BattleResult.EnemyWin;
        }
        else if (enemyUIs.Count == 0)
        {
            return BattleResult.PlayerWin;
        }
        else
        {
            return BattleResult.Nop;
        }

    }
}
