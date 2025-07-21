using TMPro;
using UnityEngine;

public class ManaUI : MonoBehaviour
{
    [SerializeField] private TMP_Text mana;
    public void UpdateManaText(int currentMana)
    {
        mana.text = currentMana.ToString();
    }
}
