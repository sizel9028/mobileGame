using System;
using System.Collections;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;

public static class CardArtLoader
{
    public static IEnumerator LoadCardArt(CardData data, Action<Sprite> onLoaded)
    {
        string resourcePath = Path.Combine(
            "CardArts",
            data.cardType.ToString(),
            data.actionType.ToString(),
            data.cardArtName  // .png 확장자 제거
        );

        ResourceRequest request = Resources.LoadAsync<Sprite>(resourcePath);
        yield return request;

        if (request.asset == null)
        {
            Debug.LogError($"[CardArtLoader] 리소스 로드 실패: {resourcePath}");
            onLoaded?.Invoke(null);
            yield break;
        }

        onLoaded?.Invoke((Sprite)request.asset);
    }
}
