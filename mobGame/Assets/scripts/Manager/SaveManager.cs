using UnityEngine;
using UnityEngine.Windows;
using System.IO;

public class SaveManager : MonoBehaviour
{
    //PlayerData 클래스의 정보를 저장하고 불러옴
    public static SaveManager saveManager;
    private string savePath;

    void Awake()
    {
        if (saveManager == null)
        {
            saveManager = this;
            DontDestroyOnLoad(gameObject);
            savePath = Application.persistentDataPath + "/save.json";
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void Save(PlayerData data)
    {
        string json = JsonUtility.ToJson(data, true);
        System.IO.File.WriteAllText(savePath, json);
    }

    public PlayerData Load()
    {
        if (System.IO.File.Exists(savePath))
        {
            string json = System.IO.File.ReadAllText(savePath);
            return JsonUtility.FromJson<PlayerData>(json);      // 저장된 정보가 있으면 그 값을 반환
        }
        else
        {
            return InitData.CreateNewPlaterData();  //저장 정보가 없을 경우 새로운 정보를 생성
        }
    }

    public PlayerData CreateNew()
    {
        return InitData.CreateNewPlaterData();
    }
}
