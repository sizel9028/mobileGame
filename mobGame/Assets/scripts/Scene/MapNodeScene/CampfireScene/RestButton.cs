using UnityEngine;
using UnityEngine.UI;

public class RestButtonController : MonoBehaviour
{
    public Button restButton;

    private void Start()
    {
        restButton.onClick.AddListener(HealPlayer);
    }

    void HealPlayer()
    {
        if (GameManager.gameManager == null)
        {
            Debug.LogError("GameManager 인스턴스가 존재하지 않습니다.");
            return;
        }

        var playerData = GameManager.gameManager.playerData;

        if (playerData == null)
        {
            Debug.LogWarning("플레이어 데이터가 존재하지 않습니다.");
            return;
        }

        int healAmount = playerData.maxHp * 30 / 100;
        playerData.hp = Mathf.Min(playerData.hp + healAmount, playerData.maxHp);
        Debug.Log($"휴식! {healAmount}만큼 회복됨. 현재 HP: {playerData.hp}");
    }

}
