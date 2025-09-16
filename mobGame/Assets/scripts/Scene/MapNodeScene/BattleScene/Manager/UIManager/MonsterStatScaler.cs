using System.Collections.Generic;
using UnityEngine;

//난이도에 따라서 적의 몬스터 공격력 / hp를 증가
public static class MonsterStatScaler
{
    private static readonly Dictionary<int, (float hp, float atk)> difficultyMultipliers = new()
    {
        { 0, (1f, 1f) },   // Normal
        { 1, (1.6f, 1.3f) }, // Hard
        { 2, (2.5f, 1.5f) }, // Very Hard
        { 3, (3f, 2f) }, // Nightmare
    };

    public static void ApplyDifficultyScaling(Character character)
    {
        //적만 적용시킴
        if (character.isPlayer) return;

        //Debug.Log($"[CharacterUIManager]  outgoingDamageTotal = {character.statMultiplier.outgoingDamageTotal}");


        int difficulty = GameManager.gameManager.playerData.difficulty;

        if (!difficultyMultipliers.ContainsKey(difficulty)) return;

        var (hpMul, atkMul) = difficultyMultipliers[difficulty];

        character.maxHp = Mathf.RoundToInt(character.maxHp * hpMul);
        character.currentHp = Mathf.RoundToInt(character.currentHp * hpMul);
        character.shield = Mathf.RoundToInt(character.shield * hpMul);
        character.statMultiplier.outgoingDamageTotal = character.statMultiplier.outgoingDamageTotal * atkMul;
        //Debug.Log($"[CharacterUIManager]  outgoingDamageTotal = {character.statMultiplier.outgoingDamageTotal}");
    }

    public static void ApplyDifficultyScalingWithData(CharacterData characterData)
    {
        int difficulty = GameManager.gameManager.playerData.difficulty;

        if (!difficultyMultipliers.ContainsKey(difficulty)) return;

        var (hpMul, atkMul) = difficultyMultipliers[difficulty];

        characterData.maxHp = Mathf.RoundToInt(characterData.maxHp * hpMul);
        characterData.hp = Mathf.RoundToInt(characterData.hp * hpMul);
        characterData.atkCoef = atkMul * characterData.atkCoef;
    }
}
