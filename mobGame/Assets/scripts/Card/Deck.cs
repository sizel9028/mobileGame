using System.Collections.Generic;
using UnityEngine;

public enum DeckType
{
    //TODO 누구의 덱인지
    Player, Enemy
}
public class Deck : MonoBehaviour
{
    public List<CardData> allCards = new();
    public DeckType deckType;

}
