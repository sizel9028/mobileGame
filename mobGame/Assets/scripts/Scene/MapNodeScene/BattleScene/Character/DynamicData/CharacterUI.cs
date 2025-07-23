using TMPro;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System;

public class CharacterUI : MonoBehaviour
{
    // 체력 바 UI 슬라이더
    [SerializeField] private Slider hpBar;

    // 체력 수치 표시용 텍스트 (예: "30 / 30")
    [SerializeField] private TextMeshProUGUI hpText;

    // 방어력 수치 표시용 텍스트
    [SerializeField] private TextMeshProUGUI shieldText;

    // 이미지를 표시할 UI 이미지
    [SerializeField] public Image characterImage;
    [SerializeField] public Image characterBoldImage;

    [SerializeField] private Material redMaterial; // border이미지 빨간색 만들기

    [Header("데이터 정보")]

    public bool isPlayer;   // 위치를 알아야함


    // 적의 최대 체력과 현재 체력
    public int MaxHealth { get; private set; }
    public int CurrentHealth { get; private set; }

    // 방어력 수치 (대미지 감소용)
    public int Shield { get; private set; }
    public Character character;

    private string chName;


    public void Setup()
    {
        Setup(character);  //쉴드 피 등 세팅
    }

    public void Setup(Character data)  // 완전 초기 셋팅
    {
        //TODO 캐릭터 정보를 바탕으로 UI를 셋업함   
        character = data;
        isPlayer = data.isPlayer;

        MaxHealth = data.maxHp;
        CurrentHealth = data.currentHp;
        Shield = data.shield;

        //hp 정보 셋팅
        hpBar.maxValue = MaxHealth;
        hpBar.value = CurrentHealth;

        //텍스트 갱신
        UpdateHealthText();
        UpdateShieldText(Shield);

        //아트가 이전에 호출했던거랑 이름이 다르면 로드하고 같으면 로드하지 않음
        if (chName == null || chName != character.characterArtName)
        {
            StartCoroutine(CharacterArtLoader.LoadCharacterArt(data.characterArtName, (sprite) =>
            {
                if (sprite != null)
                {
                    characterImage.sprite = sprite;
                    characterBoldImage.sprite = sprite;
                }
                else
                {
                    Debug.LogError($"[CharacterUI] 캐릭터 아트 로딩 실패: {data.characterArtName}");
                }
            }));
            chName = character.characterArtName;
        }
    }


    private void UpdateHealthText()
    {
        hpText.text = $"{CurrentHealth} / {MaxHealth}";
    }

    private void UpdateShieldText(int shieldAmount)
    {
        shieldText.text = shieldAmount.ToString();
    }

    // 데미지 받는 함수
    public void Damage()
    {

        // 흔들림 연출 (UI용)
        RectTransform rectTransform = GetComponent<RectTransform>();
        if (rectTransform != null)
        {
            rectTransform.DOShakeAnchorPos(0.2f, 30f, 10, 90, false);  // 진폭값은 상황에 맞게 조정
        }

    }

    //선택됬을때 테두리 색 변경
    public void SetOutlineColor(bool isRed)
    {
        if (characterBoldImage == null) return;

        if (isRed)
        {
            Material redOverride = new Material(Shader.Find("UI/SolidColorWithAlpha"));
            redOverride.color = Color.red;

            characterBoldImage.material = redOverride;
        }
        else
        {
            characterBoldImage.material = null;             // 기본 머티리얼로 되돌림(검은색 만들기 위함)
            characterBoldImage.color = Color.black;
        }
    }

    public void DestroySelf()
    {
        if (characterImage.sprite != null)
        {
            Destroy(characterImage.sprite.texture);
            Destroy(characterImage.sprite);

        }

        Destroy(gameObject);
    }
}
