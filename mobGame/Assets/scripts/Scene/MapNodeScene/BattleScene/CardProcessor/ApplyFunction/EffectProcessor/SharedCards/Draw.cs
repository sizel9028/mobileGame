using System.Collections;
using UnityEngine;

public partial class CardEffectProcessor
{
    private void ApplyDraw(float amount)
    {
        int drawCount = Mathf.RoundToInt(amount);

        if (!casterUI.isPlayer)
        {
            Debug.LogWarning("[ApplyDraw] 적은 드로우하지 않음");
            return;
        }

        casterUI.StartCoroutine(DelayedDraw(drawCount, 0.4f));
    }

    private IEnumerator DelayedDraw(int count, float delay)
    {

        for (int i = 0; i < count; ++i)
        {
            yield return new WaitForSeconds(delay);
            DeckManager.Instance.DrawCard();
        }
    }
}