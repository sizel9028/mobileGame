using TMPro;
using UnityEngine;

public class GachaSceneManager : MonoBehaviour
{
    public TMP_Text clearText;  // Clear보여줌
    //얼만큼의 재화 hp가 증가했는지
    public TMP_Text hpText;
    public TMP_Text goldText;

    //TODO 버튼 추가(카드 보상 받기, 아무 보상 받지 않기)

    private void DisplayClearMsg()
    {
        clearText.text = "Clear";
        
    }
}
