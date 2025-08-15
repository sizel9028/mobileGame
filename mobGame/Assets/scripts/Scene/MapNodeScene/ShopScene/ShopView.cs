using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class ShopView : MonoBehaviour
{
    public List<CardUI> cards = new();
    private CardUI selectedCard = null;
    private CardUI zoomedCard = null;

    private Vector2 startPos;
    private Vector3 originalPos;
    private Vector3 originalScale;
    private Quaternion originalRot;
    private int originalSiblingIndex;

    private Coroutine longPressRoutine;
    private float longPressTime = 0.8f;
    private bool isZooming = false;
    private bool isZoomed = false;

    public CardUI GetSelectedCard() => selectedCard;

    public void AddCard(CardUI card)
    {
        cards.Add(card);
    }

    public void OnCardDown(CardUI card)
    {
        if (isZooming || isZoomed) return;

        startPos = Input.mousePosition;
        originalPos = card.transform.position;
        originalScale = card.transform.localScale;
        originalRot = card.transform.rotation;
        originalSiblingIndex = card.transform.GetSiblingIndex();

        zoomedCard = card;

        longPressRoutine = StartCoroutine(LongPressRoutine());
    }

    public void OnCardUp(CardUI card)
    {
        if (longPressRoutine != null)
        {
            StopCoroutine(longPressRoutine);
            longPressRoutine = null;
        }


        if (isZoomed)
        {
            ResetZoom();
        }
        else
        {

            if (selectedCard != null)
            {
                selectedCard.SetSelected(false);
            }

            card.SetSelected(true);
            selectedCard = card;

            ShopSceneManager.Instance.SetDescText();  //가격 배치
        }

        isZooming = false;
        isZoomed = false;
        zoomedCard = null;
    }

    private IEnumerator LongPressRoutine()
    {
        isZooming = true;
        float timer = 0f;

        while (timer < longPressTime)
        {
            float dist = Vector2.Distance(Input.mousePosition, startPos);
            if (dist > 40f)
            {
                isZooming = false;
                yield break;
            }

            timer += Time.deltaTime;
            yield return null;
        }

        ZoomCard(zoomedCard);
    }

    private void ZoomCard(CardUI card)
    {
        isZoomed = true;
        isZooming = false;
        var rect = card.GetComponent<RectTransform>();

        rect.SetAsLastSibling();
        rect.DOAnchorPos(new Vector2(-400f, 0f), 0.25f).SetUpdate(true);
        rect.DOScale(originalScale * 1.8f, 0.25f).SetUpdate(true);
        rect.DORotate(Vector3.zero, 0.25f).SetUpdate(true);
    }

    private void ResetZoom()
    {
        if (zoomedCard == null) return;

        isZooming = false;
        isZoomed = false;

        var rect = zoomedCard.GetComponent<RectTransform>();
        rect.SetSiblingIndex(originalSiblingIndex);
        rect.DOMove(originalPos, 0.25f).SetUpdate(true);
        rect.DOScale(originalScale, 0.25f).SetUpdate(true);
        rect.DORotate(originalRot.eulerAngles, 0.25f).SetUpdate(true);
    }

    public void RemoveSelectedCard()
    {
        if (selectedCard == null)
            return;

        cards.Remove(selectedCard);

        selectedCard.DestroyCard();

        selectedCard = null;
    }
}
