using UnityEngine;
using UnityEngine.SceneManagement;

public class CfManager : MonoBehaviour
{

    void Start()
    {
        bool skipCf = GameManager.gameManager.CfSceneUpgrade;

        if (skipCf)
        {
            GameManager.gameManager.CfSceneUpgrade = false; // 초기화 시킴

            //TODO 씬을 저장후 다음 노드로 넘김

            SaveManager.saveManager.SaveAll();  //저장하고
            SceneManager.LoadScene("stageScene");
        }

    }

}
