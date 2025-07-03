using System;
using System.Collections.Generic;
using UnityEngine;

public class CardGenerator : MonoBehaviour
{
    public static Deck LoadDeck(int stage, int theme, int level)
    {
        string path = $"Decks/Stage{stage}/Theme{theme}/Level{level}";
        // Decks > Stage1 > Theme1 > Level1.csv
        //Execept 0 0 0일때는 초기 플레이어 카드

        var deck = new Deck
        {
            //deckType = (stage == 0) ? DeckType.Player : DeckType.Enemy,
            cards = new List<CardData>()
        };

        TextAsset csv = Resources.Load<TextAsset>($"{path}/data");
        if (csv == null)
        {
            return deck;
        }

        //TODO CardData를 바탕으로 csv 파일 분석

        string[] lines = csv.text.Split('\n');
        for (int i = 1; i < lines.Length; ++i)
        {
            string line = lines[i].Trim();
            if (string.IsNullOrEmpty(line)) continue;

            string[] tokens = line.Split(',');
            if (tokens.Length < 10)
            {
                Debug.LogWarning($"[CardGenerator] 잘못된 CSV 라인: {line}");
                continue;
            }

            try
            {
                var card = new CardData
                {
                    nameKey = tokens[0],
                    descriptionKey = tokens[1],
                    cardArtName = tokens[2],
                    path = path,
                    cardType = Enum.Parse<CardType>(tokens[3]),
                    actionType = Enum.Parse<ActionType>(tokens[4]),
                    cardTarget = Enum.Parse<CardTarget>(tokens[5]),
                    rare = Enum.Parse<Rare>(tokens[6]),
                    cost = int.Parse(tokens[7]),
                    damage = int.Parse(tokens[8]),
                    statusEffect = Enum.Parse<StatusEffect>(tokens[9])
                };

                card.LoadArt();
                deck.cards.Add(card);
            }
            catch (Exception err)
            {
                Debug.LogError($"[CardGenerator] 카드 생성 중 오류 발생 (줄 {i + 1}): {err.Message}");
            }
        }

        return deck;
    }
}
