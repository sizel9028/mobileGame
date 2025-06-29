using System.Collections.Generic;
using UnityEngine;

public class CardGenerator : MonoBehaviour
{
    public static Deck LoadDeck(int stage, int theme, int level)
    {
        string path = $"Decks/Stage{stage}/Theme{theme}/Level{level}";
        // Decks > Stage1 > Theme1 > Level1
        //Execept 0 0 0일때는 초기 플레이어 카드

        var deck = new Deck
        {
            deckType = (stage == 0) ? DeckType.Player : DeckType.Enemy,
            allCards = new List<CardData>()
        };

        TextAsset csv = Resources.Load<TextAsset>(path);
        if (csv == null)
        {
            return deck;
        }

        //TODO CardData를 바탕으로 csv 파일 분석
        return null;
    }
}
