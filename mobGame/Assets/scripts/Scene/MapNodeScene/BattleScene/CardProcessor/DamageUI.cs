using System.Collections;
using TMPro;
using UnityEngine;


//데미지를 입었을때 캐릭터 UI 주변에 숫자를 나타냄 + miss일 경우 miss영어를 표시
public class DamageUI : Singleton<DamageUI>
{
    //prefab으로 만듬
    [SerializeField] private TextMeshProUGUI showTxtPrefab;
    [SerializeField] private GameObject textPanel;  //텍스트를 붙일 부모 오브젝트

    public void ShowDamage(CharacterUI targetUI, int damage, bool isMiss)
    {
        //만약 시뮬레이션 중이라면 데미지를 나타내지는 않음 (simRoot 밑에 있으면)
        if (targetUI.transform.parent != null && targetUI.transform.parent.name == "simRoot")
        {
            return;
        }

        string text = isMiss ? "miss" : damage.ToString();
        if (!isMiss && damage <= 0) return; //데미지를 보여주지 않음

        TextMeshProUGUI dmgTxt = Instantiate(showTxtPrefab, textPanel.transform);
        dmgTxt.text = text;
        dmgTxt.color = isMiss ? Color.white : Color.red;

        var mat = new Material(dmgTxt.fontMaterial);
        mat.SetFloat(TMPro.ShaderUtilities.ID_OutlineWidth, 0f);
        dmgTxt.fontMaterial = mat;
        //outline을 없앰(눈에 보기에 안좋음)

        RectTransform charRect = targetUI.GetComponent<RectTransform>();
        RectTransform dmgRect = dmgTxt.GetComponent<RectTransform>();

        float offsetX = Random.Range(-30f, 30f);
        float offsetY = Random.Range(-20f, 20f);

        Vector2 basePos = charRect.anchoredPosition + Vector2.up * 50f; // 캐릭터 위쪽
        dmgRect.anchoredPosition = basePos + new Vector2(offsetX, offsetY);

        StartCoroutine(FadeAndMove(dmgTxt));
    }

    private IEnumerator FadeAndMove(TextMeshProUGUI dmgTxt)
    {
        float duration = 1f;
        float time = 0f;

        RectTransform rect = dmgTxt.GetComponent<RectTransform>();
        Vector2 startPos = rect.anchoredPosition;
        Vector2 endPos = startPos + Vector2.up * 50f; // 캔버스 좌표에서 위로 50px

        Color startColor = dmgTxt.color;

        while (time < duration)
        {
            time += Time.deltaTime;
            float t = time / duration;

            rect.anchoredPosition = Vector2.Lerp(startPos, endPos, t);
            dmgTxt.color = new Color(startColor.r, startColor.g, startColor.b, 1 - t);

            yield return null;
        }

        Destroy(dmgTxt.gameObject);
    }

}
