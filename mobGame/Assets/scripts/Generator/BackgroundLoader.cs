using UnityEngine;

public static class BackgroundLoader
{
    public static Sprite LoadBackgroundSprite(int stage, bool isMap)
    {
        MapTheme theme = GameManager.gameManager.playerData.currentMap.theme;

        string folder = isMap ? "Map" : "Battle";
        string path = $"BackGround/{folder}/Stage{stage}/{theme.ToString().ToLower()}";

        Sprite sprite = Resources.Load<Sprite>(path);

        if (sprite == null)
        {
            Debug.LogWarning($"[BackgroundLoader] 배경 로드 실패: {path}");
        }

        return sprite;
    }
}
