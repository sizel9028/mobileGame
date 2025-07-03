using UnityEngine;

public class Battle : MonoBehaviour
{
    //BattleScene의 최상위 매니저

    public DeckManager deckManager;

    void Start()
    {
        deckManager.InitDeck();
    }
}
