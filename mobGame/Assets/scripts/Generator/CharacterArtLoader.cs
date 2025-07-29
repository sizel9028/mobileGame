using System;
using System.Collections;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;


public static class CharacterArtLoader
{
    public static IEnumerator LoadCharacterArt(string artName, Action<Sprite> onLoaded)
    {
        string resourcePath = Path.Combine("CharacterArts", artName); // .png 제거

        ResourceRequest request = Resources.LoadAsync<Sprite>(resourcePath);
        yield return request;

        if (request.asset == null)
        {
            Debug.LogError($"[CharacterArtLoader] 리소스 로드 실패: {resourcePath}");
            onLoaded?.Invoke(null);
            yield break;
        }

        onLoaded?.Invoke((Sprite)request.asset);
    }
}
