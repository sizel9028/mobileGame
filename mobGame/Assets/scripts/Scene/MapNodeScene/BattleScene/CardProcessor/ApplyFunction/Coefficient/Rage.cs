using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public partial class CoefficientModifier
{
    private void ApplyRage(float amount, Character caster, Character target)
    {
        // 1. 플레이어만 적용
        if (!target.isPlayer) return;

        var targetUI = CharacterUIManager.Instance.GetUI(target);

        // 2. 체력 변화
        if (amount < 0)
        {
            int hpChange = Mathf.RoundToInt(amount);
            target.currentHp -= hpChange; // 음수일때 + 값
            target.currentHp = Mathf.Clamp(target.currentHp, 0, target.maxHp);

            //캐릭터 이름 변경
            if (target.characterArtName.EndsWith("_rage"))
            {
                target.characterArtName = target.characterArtName.Substring(0, target.characterArtName.Length - "_rage".Length);
            }
        }

        // 3. 덱 변경 (플레이어 전용)
        if (amount > 0)
        {
            if (!target.characterArtName.EndsWith("_rage"))
            {
                target.characterArtName += "_rage";
            }

            Battle.Instance.StartCoroutine(ChangeDeckForRageRoutine());
        }


        // 4. UI 업데이트
        targetUI.Setup();
    }

    // Rage 전용 덱 변경 로직
    private IEnumerator ChangeDeckForRageRoutine()
    {
        var handView = DeckManager.Instance.handView;
        var handPanel = DeckManager.Instance.handPanel;

        List<CardData> upgradeCards = new List<CardData>();
        UpgradeProcessor upgradeProcessor = new UpgradeProcessor();

        foreach (var cardUI in handView.cards)
        {
            if (cardUI.data.cardType == CardType.Scroll)
            {
                continue;  //스크롤은 적용안함
            }
            CardData copy = (CardData)cardUI.data.Clone();
            upgradeProcessor.UpgradeCard(copy);
            copy.rare = Rare.TierRage;
            upgradeCards.Add(copy);
        }

        // 손패 전부 버리기
        handView.DiscardAllCards();

        // DiscardAllCards 애니메이션 시간
        float totalDelay = 1f + (handView.cards.Count * 0.05f);
        yield return new WaitForSeconds(totalDelay);

        DeckManager.Instance.DrawScrollCards();

        // Rage 카드로 손패 채우기
        foreach (var card in upgradeCards)
        {
            var newCardUI = DeckManager.Instance.cardUIManager.CreateCard(card, handPanel);
            newCardUI.SetHandView(handView);
            yield return handView.AddCard(newCardUI);
        }
    }

}
