using UnityEngine;


//룬 프로세서
public class RuneProcessor
{

    public void ProcessRuneEffect(Character character)
    {
        var runes = GameManager.gameManager.runeData;

        foreach (var rune in runes.runes)
        {
            ApplyRune(rune, character);
        }
    }

    private void ApplyRune(Rune rune, Character character)
    {
        switch (rune.mapTheme)
        {
            case MapTheme.FROST:
                Frost(rune.level);
                break;
        }
    }

    private void Frost(int level)
    {

    }

    public float GetRuneCoefficient(Rune rune)
    {
        if (rune.level <= 0) return 0f;

        switch (rune.mapTheme)
        {
            //보상 카드 드로우 횟수+
            case MapTheme.OCEAN:
                if (rune.level <= 10) return 1f;
                else if (rune.level <= 20) return 2f;
                else return 3f;

            //마나 관련일듯?
            case MapTheme.FROST:
                if (rune.level <= 10) return 1f;
                else return 2f;

            //최대 체력 증가
            case MapTheme.VOID:
                return rune.level;

            //공격시 흡혈
            case MapTheme.FOREST:
                if (rune.level <= 10) return rune.level * 0.01f;
                else return 0.1f;

            //카드 공격력 증가
            case MapTheme.VOLCANO:
                return 0f;

            //부활
            case MapTheme.RUINS:
                return 0f;

            //회피율
            case MapTheme.DESERT:
                return 0f;

        }

        return 0f;
    }


}
