using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class BattlePreviewUI : MonoBehaviour
{
    [SerializeField] private Image[] monsterImages;
    [SerializeField] private Button deckBtn;
    [SerializeField] private TMP_Text descText; //특수 능력 설명(기믹)

    [SerializeField] private TMP_Text[] monsterHpTexts;
    [SerializeField] private TMP_Text[] monsterAttacks;
    private int nodeId;

    void Awake()
    {
        deckBtn.onClick.AddListener(OnClickBtn);
    }

    public void Setup(int nodeId)
    {
        this.nodeId = nodeId;
        int level = LevelGenerator.GetLevelInfo(nodeId);
        //---test---
        //int level = 1;
        //어떤 적이 나오는지
        List<string> enemies = StageLoader.Load(GameManager.gameManager.playerData.currentMap.stageNumber, level);
        CreateEnemies(enemies);

        //TODO desc 텍스트 작성
        SetDescText(enemies);
    }

    private void CreateEnemies(List<string> enemies)
    {
        for (int i = 0; i < monsterImages.Length; i++)
        {
            if (i < enemies.Count)
            {
                string enemyName = enemies[i];
                CharacterData data = ChdataGenerator.GetData(enemyName);

                if (data == null)
                {
                    Debug.LogWarning($"[BattlePreviewUI] '{enemyName}'에 대한 CharacterData를 찾을 수 없습니다.");
                    continue;
                }

                MonsterStatScaler.ApplyDifficultyScalingWithData(data);

                int index = i;

                // 아트 비동기 로딩
                StartCoroutine(CharacterArtLoader.LoadCharacterArt(enemyName, sprite =>
                {
                    if (sprite != null)
                    {
                        monsterImages[index].sprite = sprite;
                        monsterImages[index].gameObject.SetActive(true);
                    }
                    else
                    {
                        monsterImages[index].gameObject.SetActive(false);
                    }
                }));

                monsterHpTexts[i].text = $"HP: {data.hp}";
                monsterAttacks[i].text = $"ATK x{data.atkCoef:0.##}";
            }
            else
            {
                monsterImages[i].gameObject.SetActive(false);
                monsterHpTexts[i].text = "";
                monsterAttacks[i].text = "";
            }
        }
    }

    private void SetDescText(List<string> enemies)
    {
        List<string> gimmickDescriptions = new();
        HashSet<string> handledEnemies = new();

        foreach (string enemy in enemies)
        {
            // 중복 방지
            if (handledEnemies.Contains(enemy))
                continue;

            handledEnemies.Add(enemy);

            List<Gimmick> gimmicks = GimmickLoader.GetGimmickByName(enemy);

            if (gimmicks != null && gimmicks.Count > 0)
            {
                List<string> translated = new();
                foreach (var gimmick in gimmicks)
                {
                    if (!string.IsNullOrEmpty(gimmick.gimmickName))
                    {
                        string localized = LocalizationManager.languageM.GetText(gimmick.gimmickName);
                        translated.Add(localized);
                    }
                }

                string combined = string.Join(", ", translated);

                // "특수능력 :" + 기믹 설명 붙이기
                string prefix = LocalizationManager.languageM.GetText("BattlePreview_specialAbility"); 
                gimmickDescriptions.Add($"{prefix} : {combined}");
            }
        }

        if (gimmickDescriptions.Count == 0)
        {
            descText.text = LocalizationManager.languageM.GetText("BattlePreview_nothing");
        }
        else
        {
            descText.text = string.Join("\n", gimmickDescriptions);
        }

        //폰트 설정
        descText.font = LocalizationManager.languageM.GetFont();
    }

    void OnClickBtn()
    {
        //TODO 카드 보여주는 씬으로 이동
        Debug.Log("에너미 덱을 보여줌");
        int level = LevelGenerator.GetLevelInfo(nodeId);
        int theme = (int)GameManager.gameManager.playerData.currentMap.theme;
        int stage = GameManager.gameManager.playerData.currentMap.stageNumber;
        Deck enemyDeck = CardGenerator.LoadDeck(stage, theme, level);
        
        GameManager.gameManager.CardViewCards = (SceneType.InventoryScene, enemyDeck.cards);
        SceneManager.LoadScene("CardViewerScene");
    }

}
