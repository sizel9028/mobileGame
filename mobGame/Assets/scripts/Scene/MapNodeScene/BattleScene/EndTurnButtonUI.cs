using UnityEngine;

public class EndTurnButtonUI : MonoBehaviour
{
    public void Onclick()
    {
        EnemyTurn enemyTurn = new();
        ActionSystem.Instance.Perform(enemyTurn);
    }
}
