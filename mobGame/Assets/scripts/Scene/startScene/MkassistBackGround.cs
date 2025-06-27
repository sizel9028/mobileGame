using UnityEngine;

public class MkassistBackGround : MonoBehaviour
{
    // 모바일 화면 비율을 위한 검은 배경추가, 시작시 항상 보이게 함
    private SpriteRenderer sr;
    void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
        sr.enabled = true;
        DontDestroyOnLoad(gameObject);
    }
    
}
