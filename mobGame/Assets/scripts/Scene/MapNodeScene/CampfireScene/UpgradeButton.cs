using UnityEngine;
using UnityEngine.UI;

public class UpgradeButtonController : MonoBehaviour
{
    public Button upgradeButton;

    private void Start()
    {
        upgradeButton.onClick.AddListener(UpgradeCard);
    }

    void UpgradeCard()
    {
        // TODO: 카드 강화 UI 열기 및 카드 선택 후 업그레이드 처리
        Debug.Log("카드 강화 버튼 클릭됨 - TODO: 카드 강화 기능 구현 예정");
    }
}
