using System.Collections.Generic;
using System.Linq;
using UnityEngine;

//맵이 가지는 기믹 처리
public class MapEffectProcessor : Singleton<MapEffectProcessor>
{
    public void ProcessMapEffect()
    {
        MapTheme mapTheme = GameManager.gameManager.playerData.currentMap.theme;
        int turnCount = Battle.Instance.turnCount; //현재 턴
        var turnType = Battle.Instance.turnType;
        int stage = GameManager.gameManager.playerData.currentMap.stageNumber;
        

        switch (mapTheme)
        {

            case MapTheme.FROST:
                int addMana = Random.value < 0.5f ? 1 : 0; //50% 확률로 마나 +1
                if (turnType == TurnType.PlayerTurn)
                {
                    var ui = CharacterUIManager.Instance.playerUIs[0];
                    if (ui != null)
                    {
                        ui.character.statMultiplier.addTurnMana = addMana;
                    }
                }
                else
                {
                    EnemyManaSystem.Instance.InitMana(3 + addMana);
                }

                break;

            case MapTheme.FOREST:

                int minHeal = 1 + stage * 2;
                int maxHeal = 2 + stage * 4;
                float healValue = Random.Range(minHeal, maxHeal + 1);

                CardData healCard = new CardData();
                healCard.effectMap = new Dictionary<string, float> { { "Heal", healValue } };
                healCard.cardTarget = CardTarget.allPlayer;

                CardEffectProcessor cardEffectProcessor = new CardEffectProcessor();
                if (turnType == TurnType.EnemyTurn)
                {
                    var rawEnemyUIs = CharacterUIManager.Instance.enemyUIs;
                    var enemyUIs = rawEnemyUIs.Where(ui => ui != null).ToList();
                    cardEffectProcessor.ProcessCardEffect(healCard, null, enemyUIs);
                }
                break;

            case MapTheme.OCEAN:
                //보상 설정에 셋팅되어 있음
                break;

            case MapTheme.DESERT:
                //passive Processor에서 넣어둠
                break;

            case MapTheme.VOLCANO:
                //passive Processor에 넣어둠
                break;

            case MapTheme.RUINS:
                if (turnType == TurnType.PlayerTurn)
                {
                    int shieldLoss = Mathf.Max(1, turnCount);
                    var rawPlayerUIs = CharacterUIManager.Instance.playerUIs;
                    var playerUIs = rawPlayerUIs.Where(ui => ui != null).ToList();

                    foreach (var ui in playerUIs)
                    {
                        if (ui.character.shield > 0)
                        {
                            ui.character.shield = Mathf.Max(0, ui.character.shield - shieldLoss);
                            ui.Setup();  // ui 적용됨
                        }
                    }
                }
                else
                {
                    int shieldLoss = Mathf.Max(1, turnCount);
                    var rawEnemyUIs = CharacterUIManager.Instance.enemyUIs;
                    var enemyUIs = rawEnemyUIs.Where(ui => ui != null).ToList();

                    foreach (var ui in enemyUIs)
                    {
                        if (ui.character.shield > 0)
                        {
                            ui.character.shield = Mathf.Max(0, ui.character.shield - shieldLoss);
                            ui.Setup();
                        }
                    }
                }
                break;

            case MapTheme.VOID:
                if (turnType == TurnType.PlayerTurn)
                {
                    int maxHpLoss = Mathf.Max(1, stage);
                    var rawPlayerUIs = CharacterUIManager.Instance.playerUIs;
                    var playerUIs = rawPlayerUIs.Where((ui, idx) => ui != null && idx != 0).ToList();
                    var playerUI = rawPlayerUIs[0];
                    if (playerUI == null) return; // 본 플레이어가 null일 경우 리턴

                    foreach (var ui in playerUIs)
                    {
                        if (ui.character.maxHp > 1)
                        {
                            ui.character.maxHp = Mathf.Max(1, ui.character.maxHp - maxHpLoss);

                            if (ui.character.currentHp > ui.character.maxHp)
                            {
                                ui.character.currentHp = ui.character.maxHp;
                            }

                            ui.Setup();
                        }
                    }

                    //이러면 패시브로 얻은 추가 hp > decrease에 증가하고 > 다시 합하는 로직을 하면 의도한 최대체력보다 늘어남

                    var totalLoss = GameManager.gameManager.playerData.DecreaseMaxHp;
                    var maxHp = playerUI.character.maxHp;

                    float minRate = Mathf.Max(0, 70 - stage * 10) / 100f;
                    int minAllowedHp = Mathf.CeilToInt((totalLoss + maxHp) * minRate);

                    int newMaxHp = maxHp - maxHpLoss;
                    if (maxHp < minAllowedHp)
                    {
                        newMaxHp = maxHp; //진행하지 않음
                    }

                    if (newMaxHp < minAllowedHp && maxHp >= minAllowedHp) newMaxHp = minAllowedHp;

                    GameManager.gameManager.playerData.DecreaseMaxHp += maxHp - newMaxHp;
                    //Debug.Log($"[VOID] DecreaseMaxHp 누적값: {GameManager.gameManager.playerData.DecreaseMaxHp}");

                    playerUI.character.maxHp = newMaxHp;
                    if (playerUI.character.currentHp > maxHp)
                    {
                        playerUI.character.currentHp = newMaxHp;
                    }

                    playerUI.Setup();
                }
                else
                {
                    int maxHpLoss = Mathf.Max(1, stage);
                    var rawEnemyUIs = CharacterUIManager.Instance.enemyUIs;
                    var enemyUIs = rawEnemyUIs.Where(ui => ui != null).ToList();

                    foreach (var ui in enemyUIs)
                    {
                        if (ui.character.maxHp > 1)
                        {
                            ui.character.maxHp = Mathf.Max(1, ui.character.maxHp - maxHpLoss);

                            if (ui.character.currentHp > ui.character.maxHp)
                            {
                                ui.character.currentHp = ui.character.maxHp;
                            }

                            ui.Setup();
                        }
                    }
                }
                break;
        }
    }

}
