using System;
using System.Collections;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;


public static class CharacterArtLoader
{
    public static IEnumerator LoadCharacterArt(string artName, Action<Sprite> onLoaded)
    {
        string relativePath = Path.Combine("CharacterArt", artName + ".png");
        string fullPath = Path.Combine(Application.streamingAssetsPath, relativePath);

#if UNITY_ANDROID && !UNITY_EDITOR
        UnityWebRequest www = UnityWebRequestTexture.GetTexture(fullPath);
        yield return www.SendWebRequest();

        if (www.result != UnityWebRequest.Result.Success)
        {
            Debug.LogError($"[CharacterArtLoader] 로딩 실패: {www.error}");
            onLoaded?.Invoke(null);
            yield break;
        }

        Texture2D tex = DownloadHandlerTexture.GetContent(www);
#else
        if (!File.Exists(fullPath))
        {
            Debug.LogError($"[CharacterArtLoader] 파일 없음: {fullPath}");
            onLoaded?.Invoke(null);
            yield break;
        }

        byte[] bytes = File.ReadAllBytes(fullPath);
        Texture2D tex = new Texture2D(2, 2);
        tex.LoadImage(bytes);
#endif

        Sprite sprite = Sprite.Create(
            tex,
            new Rect(0, 0, tex.width, tex.height),
            new Vector2(0.5f, 0.5f)
        );

        onLoaded?.Invoke(sprite);
    }
}
