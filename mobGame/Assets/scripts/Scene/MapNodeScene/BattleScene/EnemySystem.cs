using System.Collections;
using UnityEngine;

public class EnemySystem : MonoBehaviour
{
    void OnEnable()
    {
        ActionSystem.AttachPerformer<EnemyTurn>(EnemyTurnPerform);
    }

    void OnDisable()
    {
        ActionSystem.DetachPerformer<EnemyTurn>();
    }
    // Perform

    private IEnumerator EnemyTurnPerform(EnemyTurn enemyTurn)
    {
        Debug.Log("Enemy Turn");
        yield return new WaitForSeconds(2f);
        Debug.Log("End Enemy Turn");
    }
}
