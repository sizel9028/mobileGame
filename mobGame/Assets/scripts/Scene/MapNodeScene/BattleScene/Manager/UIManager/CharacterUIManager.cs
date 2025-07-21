using System.Collections.Generic;
using UnityEngine;

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

        Character character = new Character();
        character.Setup(data);
        character.isPlayer = isPlayer;

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
}
