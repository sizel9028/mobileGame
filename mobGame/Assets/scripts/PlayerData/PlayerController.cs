using System;
using UnityEngine.UI;
using UnityEngine;
using TMPro;

public class PlayerController : MonoBehaviour
{
    // HpBar Slider를 연동하기 위한 Slider 객체
    [SerializeField] private Slider _hpBar;
    [SerializeField] private TextMeshProUGUI hpText;       // HP 텍스트
    [SerializeField] private TextMeshProUGUI shieldText;   // 방어력 텍스트
    [SerializeField] private Image playerUIImage;           // 이미지 추가


    // 플레이어의 HP
    private int _hp;

    public int Hp
    {
        get => _hp;
        // HP는 PlayerController에서만 수정하도록 private으로 처리
        private set => _hp = Mathf.Clamp(value, 0, _hp);
    }

    private void Awake()
    {
        // 플레이어의 HP 값을 초기 세팅합니다.
        _hp = 100;
        // MaxValue를 세팅하는 함수입니다.
        SetMaxHealth(_hp);
        // 처음 텍스트 업데이트
        UpdateHPText();
    }

    // 최대 체력을 설정하고, 체력바의 최대값과 현재값을 설정
    public void SetMaxHealth(int health)
    {
        _hpBar.maxValue = health;
        _hpBar.value = health;
    }

    // 플레이어가 대미지를 받으면 대미지 값을 전달받아 HP에 반영합니다.
    public void GetDamage(int damage)
    {
        int getDamagedHp = Hp - damage;
        Hp = getDamagedHp;
        _hpBar.value = Hp;
        UpdateHPText();  // 체력 텍스트 갱신
    }

    // HP 텍스트를 "현재 HP / 최대 HP" 형식으로 업데이트하는 함수
    public void UpdateHPText()
    {
        hpText.text = $"{Hp} / {_hpBar.maxValue}";  // 현재 HP / 최대 HP
    }

    // 방어력 텍스트를 업데이트하는 함수
    public void UpdateShieldText(int shieldAmount)
    {
        shieldText.text = shieldAmount.ToString();
    }
}
