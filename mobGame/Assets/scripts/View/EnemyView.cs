using TMPro;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class EnemyView : MonoBehaviour
{
    // 체력 바 UI 슬라이더
    [SerializeField] private Slider hpBar;

    // 체력 수치 표시용 텍스트 (예: "30 / 30")
    [SerializeField] private TextMeshProUGUI hpText;

    // 방어력 수치 표시용 텍스트
    [SerializeField] private TextMeshProUGUI shieldText;

    // 적의 이미지를 표시할 UI 이미지
    [SerializeField] private Image characterImage;

    // 적의 최대 체력과 현재 체력
    public int MaxHealth { get; private set; }
    public int CurrentHealth { get; private set; }

    // 방어력 수치 (대미지 감소용)
    public int Shield { get; private set; }

    public void Setup(EnemyData enemyData)
    {
        // EnemyData에서 필요한 정보 꺼내서 기존 Setup에 전달
        Setup(enemyData.health, enemyData.sprite);
    }

    /// <summary>
    /// 적 정보를 초기화하는 함수
    /// </summary>
    // <param name="health">최대 체력</param>
    // <param name="sprite">적 이미지 (선택)</param>
    public void Setup(int health, Sprite sprite = null)
    {
        // 체력 설정
        MaxHealth = CurrentHealth = health;

        // 체력바 슬라이더 설정
        hpBar.maxValue = MaxHealth;
        hpBar.value = CurrentHealth;

        // 텍스트 UI 갱신
        UpdateHealthText();
        UpdateShieldText(0);  // 초기 방어력은 0

        // 이미지 설정 (입력된 경우에만)
        if (sprite != null)
            characterImage.sprite = sprite;
    }

    /// <summary>
    /// 적이 대미지를 받을 때 실행
    /// 방어력을 고려하여 실제 피해량 계산
    /// </summary>
    // <param name="damage">입력된 대미지</param>
    public void GetDamage(int damage)
    {
        int effectiveDamage = Mathf.Max(damage - Shield, 0); // 방어력 적용
        CurrentHealth = Mathf.Clamp(CurrentHealth - effectiveDamage, 0, MaxHealth); // 체력 감소
        hpBar.value = CurrentHealth;

        UpdateHealthText(); // 텍스트 갱신
    }

    /// <summary>
    /// 방어력을 설정하고 UI에 표시
    /// </summary>
    // <param name="amount">방어력 수치</param>
    public void SetShield(int amount)
    {
        Shield = amount;
        UpdateShieldText(Shield);
    }

    /// <summary>
    /// 체력 텍스트를 현재 체력 / 최대 체력 형식으로 갱신
    /// </summary>
    private void UpdateHealthText()
    {
        hpText.text = $"{CurrentHealth} / {MaxHealth}";
    }

    /// <summary>
    /// 방어력 텍스트를 숫자로 갱신
    /// </summary>
    // <param name="shieldAmount">현재 방어력</param>
    private void UpdateShieldText(int shieldAmount)
    {
        shieldText.text = shieldAmount.ToString();
    }
    
    // 데미지 받는 함수
    public void Damage(int amount)
    {

        // 흔들림 연출 (UI용)
        RectTransform rectTransform = GetComponent<RectTransform>();
        if (rectTransform != null)
        {
            rectTransform.DOShakeAnchorPos(0.2f, 30f, 10, 90, false);  // 진폭값은 상황에 맞게 조정
        }

    // 추후 체력 감소 처리도 여기서 추가할 수 있음

    }
}
