using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SubMaterialManager : Singleton<SubMaterialManager>
{
    //서브 매터리얼 (분노, 뼈) 와 같은 수치를 보여주는 매니저

    [SerializeField] private TextMeshProUGUI subTxt;
    [SerializeField] private Image materialImage;

    [SerializeField] private TextMeshProUGUI goldTxt;

    [SerializeField] private Sprite boneImage;
    [SerializeField] private Sprite rageImage;

    //[SerializeField] private GameObject subMaterial; //부모 서브 매터리얼 오브젝트

    private bool isSetup = true;

    void Start()
    {
        //TODO 만약 서브 매터리얼을 쓰지 않는 캐릭터면 txt를 숨기기

        //텍스트의 outline을 없애기
        var mat = new Material(subTxt.fontMaterial);
        mat.SetFloat(TMPro.ShaderUtilities.ID_OutlineWidth, 0f);
        subTxt.fontMaterial = mat;

        var mat1 = new Material(goldTxt.fontMaterial);
        mat1.SetFloat(TMPro.ShaderUtilities.ID_OutlineWidth, 0f);
        goldTxt.fontMaterial = mat1;

        UpdateTxt();
    }

    private void HideMaterial()
    {
        subTxt.gameObject.SetActive(false);
        materialImage.gameObject.SetActive(false);
    }

    private void SetImage()
    {
        
    }

    public void UpdateTxt()
    {
        var ui = CharacterUIManager.Instance.playerUIs[0];

        if (ui == null) return;

        int count;

        //ui내부의 뼈 또는 분노 값으로 함
        switch (ui.character.characterArtName)
        {
            case "summoner":
                count = (int)ui.character.statMultiplier.CorpseCount;
                Debug.Log("뼈 갯수" + count.ToString());
                if (isSetup)
                {
                    materialImage.sprite = boneImage;
                }
                break;

            case "berserker":
                count = (int)ui.character.statMultiplier.rage;
                if (isSetup)
                {
                    materialImage.sprite = rageImage;
                }
                break;

            default:
                count = 0;
                break;
        }
        isSetup = false;
        subTxt.text = ": " + count.ToString();
        var gold = GameManager.gameManager.playerData.gold;
        goldTxt.text = ": " + gold.ToString();
    }
}
