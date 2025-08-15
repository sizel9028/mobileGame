using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GoldHpSetup : MonoBehaviour
{
    [SerializeField] private TMP_Text hpText;
    [SerializeField] private TMP_Text goldText;
    [SerializeField] private Image backgroundPanel;

    void Start()
    {
        int hp = GameManager.gameManager.playerData.characterData.hp;
        int gold = GameManager.gameManager.playerData.gold;

        hpText.text = $"HP: {hp}";
        goldText.text = $"GOLD: {gold}";

        Color c = backgroundPanel.color;
        c.a = 0.5f;
        backgroundPanel.color = c;
    }
}
