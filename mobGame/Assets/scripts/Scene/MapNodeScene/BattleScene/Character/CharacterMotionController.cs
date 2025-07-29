using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

//모션 담당 클래스
public class CharacterMotionController
{
    //공격 모션
    public IEnumerator AttackRoutine(CharacterUI character)
    {
        RectTransform rect = character.GetComponent<RectTransform>();
        Vector2 originalPos = rect.anchoredPosition;

        Vector2 dir = character.isPlayer ? Vector2.right : Vector2.left;
        Vector2 targetPos = originalPos + dir * 50f;

        //이동
        Tween moveOut = rect.DOAnchorPos(targetPos, 0.15f);
        yield return moveOut.WaitForCompletion();

        // 다시 원래 자리로 복귀
        Tween moveBack = rect.DOAnchorPos(originalPos, 0.25f);
        yield return moveBack.WaitForCompletion();
    }

    //죽음 모션
    public IEnumerator DeathRoutine(CharacterUI character)
    {
        Sequence deathSequence = DOTween.Sequence();

        if (character.characterImage != null)
        {
            deathSequence.Join(character.characterImage.rectTransform.DOScale(Vector3.zero, 0.3f));
            deathSequence.Join(character.characterImage.DOFade(0f, 0.3f));
        }

        if (character.characterBoldImage != null)
        {
            deathSequence.Join(character.characterBoldImage.rectTransform.DOScale(Vector3.zero, 0.3f));
            deathSequence.Join(character.characterBoldImage.DOFade(0f, 0.3f));
        }

        deathSequence.Join(character.transform.DOScale(Vector3.zero, 0.3f));

        yield return deathSequence.WaitForCompletion();
    }

    //스폰 모션
    public IEnumerator SpawnRoutine(CharacterUI characterUI, float duration = 0.5f)
    {
        Sequence spawnSequence = DOTween.Sequence();

        if (characterUI.characterImage != null)
        {
            characterUI.characterImage.rectTransform.localScale = Vector3.zero;
            var color = characterUI.characterImage.color;
            characterUI.characterImage.color = new Color(color.r, color.g, color.b, 0f);

            spawnSequence.Join(characterUI.characterImage.rectTransform.DOScale(Vector3.one, duration).SetEase(Ease.OutBack));
            spawnSequence.Join(characterUI.characterImage.DOFade(1f, duration));
        }

        if (characterUI.characterBoldImage != null)
        {
            characterUI.characterBoldImage.rectTransform.localScale = Vector3.zero;
            var color = characterUI.characterBoldImage.color;
            characterUI.characterBoldImage.color = new Color(color.r, color.g, color.b, 0f);

            spawnSequence.Join(characterUI.characterBoldImage.rectTransform.DOScale(Vector3.one, duration).SetEase(Ease.OutBack));
            spawnSequence.Join(characterUI.characterBoldImage.DOFade(1f, duration));
        }

        yield return spawnSequence.WaitForCompletion();
    }

}

