using UnityEngine;

public class Rune
{
    public int level = 0;
    public MapTheme mapTheme; //맵 Theme에 따라서 어떤 룬인지 결정

    public string GetNameKey()
    {
        return $"rune_{mapTheme.ToString().ToLower()}_name";
    }

    public string GetDescKey()
    {
        return $"rune_{mapTheme.ToString().ToLower()}_desc";
    }

    public string GetArtPath()
    {
        return $"RuneArts/{mapTheme.ToString().ToLower()}";
    }
}
