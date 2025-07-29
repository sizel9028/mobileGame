using UnityEngine;

public class EndTurnButtonUI : MonoBehaviour
{

    public void Onclick()
    {
        Debug.Log("End Turn 버튼 클릭됨"); // 로그 확인용
        //코루틴은 파괴안되는 Battle에서 돌림
        Battle.Instance.StartCoroutine(Battle.Instance.EnemyTurn());
    }

    //버튼 보여주거나 숨기는 용도
    public void SetActive(bool isActive)
    {
        gameObject.SetActive(isActive);
    }

}
