using UnityEngine;
using UnityEngine.UI;

public class CancelZoneMarker : MonoBehaviour
{
    //캔슬존임을 판단하기 위한 클래스
    [Header("UI")]
    [SerializeField] private Image borderImage;
    [SerializeField] private Image backgroundImage;

    [Header("색상")]
    [SerializeField] private Material redMaterial;

    public void SetColor(bool isSelect)
    {
        if (borderImage != null)
        {
            if (isSelect)
            {
                borderImage.color = Color.white;
                borderImage.material = redMaterial;
            }
            else
            {
                borderImage.color = Color.black;
                borderImage.material = null;
            }
        }
    }
}
