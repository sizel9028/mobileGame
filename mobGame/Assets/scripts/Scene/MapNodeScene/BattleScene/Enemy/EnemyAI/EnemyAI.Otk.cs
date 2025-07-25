using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using UnityEditor.Rendering;
using UnityEngine;

public partial class EnemyAI
{
    private Transform simRoot; //시뮬레이션 UI 모아두는 ROOT
    
    //현재 패로 원턴킬이 가능한지 체크해줌 (원턴킬이 아닐경우에도 데미지를 가장 많이 입히는 List순서를 반환)
    private (List<CardData>, bool isOtk) OneTurnKill(List<CardData> handCards)
    {
        var relevantCards = handCards
        .Where(card =>
        {
            var type = Classify(card);
            return type == EnemyPlayType.Attack || type == EnemyPlayType.BuffSelf || type == EnemyPlayType.DebuffEnemy;
        })
        .ToList();

        List<CardData> bestCombo = null;
        int minCh = 3;  //최고 콤보일때 필드위에 캐릭터
        int maxDamage = -1; //최고 콤보일때 데미지 

        foreach (var perm in GetPermutations(relevantCards))
        {
            var (usedCombo, remainCh, totalDamage) = SimCardCombo(perm);

            if (usedCombo.Count == 0) { continue; }


            if (remainCh == 0)
            {
                return (usedCombo, true);
            }

            if (remainCh < minCh || (remainCh == minCh && totalDamage > maxDamage))
            {
                bestCombo = usedCombo;
                minCh = remainCh;
                maxDamage = totalDamage;
            }
        }

        return (bestCombo, false);
    }

    private (List<CardData> combo, int minCh, int totalDamage) SimCardCombo(List<CardData> combo)
    {
        //실제 적과 에너미를 복사(이후부터는 에너미가 Player라고 생각 상대적으로)
        CloneFieldCharacters(out var simEnemyUIs, out var simPlayerUIs);
        // 마나를 다시 리필
        EnemyManaSystem.Instance.refillMana();

        List<CardData> usedCombo = new();

        int totalDamage = 0;

        foreach (var card in combo)
        {
            //TODO 사용한 가능한 카드인지 검사
            if (!CardValidator.IsCardAble(card, isPlayer: false)) continue; //불가능하면 패스

            SpendCostSim(card);

            int beforeHp = simPlayerUIs.Sum(ui => ui.character.currentHp + ui.character.shield);  //콤보전 hp+실드 기록

            //무슨 카드 썻는지 로그
            foreach (var kvp in card.effectMap)
            {
                Debug.Log($"[SimCardCombo] └ 효과: {kvp.Key}, 수치: {kvp.Value}");
            }

            ProcessCardWithSim(card, simEnemyUIs, simPlayerUIs); //상대 기준

            int afterHp = simPlayerUIs.Sum(ui => ui.character.currentHp + ui.character.shield);
            totalDamage += beforeHp > afterHp ? beforeHp - afterHp : 0;

            //0이하의 hp 매번 체크
            (simPlayerUIs, simEnemyUIs) = CheckHp(simPlayerUIs, simEnemyUIs);
            //콤보에 사용한 카드를 리스트에 넣음
            usedCombo.Add(card);

        }

        Debug.Log("시뮬레이션 종료");
        return (usedCombo, simPlayerUIs.Count, totalDamage); //살아남은 플레이어 + 총 데미지 반환
    }

    private void SpendCostSim(CardData card)
    {
        if (card.costType == CostType.Mana)
        {
            EnemyManaSystem.Instance.SpendMana(card.cost);
        }

        if (card.costType == CostType.Hp)
        {
            //먼저 hp가 가능한지 체크부터 함 (무조건 있음, cardValidator을 통과했기에)
            var candidates = CharacterUIManager.Instance.enemyUIs
                .Where(ui => ui?.character != null && ui.character.currentHp > card.cost)
                .ToList();

            if (candidates.Count == 0) return;

            var target = candidates
                .OrderByDescending(ui => ui.character.currentHp)
                .First();

            target.character.currentHp -= card.cost;
        }
    }

    //Hp가 0이하이면 빼버림
    private (List<CharacterUI> playerResult, List<CharacterUI> enemyResult) CheckHp(
    List<CharacterUI> simPlayerUIs, List<CharacterUI> simEnemyUIs)
    {
        //HP가 0 이하인 캐릭터 제거
        simPlayerUIs = simPlayerUIs.Where(ui => ui.character.currentHp > 0).ToList();
        simEnemyUIs = simEnemyUIs.Where(ui => ui.character.currentHp > 0).ToList();

        return (simPlayerUIs, simEnemyUIs);
    }


    //이건 상대적이 아닌 실제 플레이어 에너미를 복사함
    private void CloneFieldCharacters(out List<CharacterUI> enemyClones, out List<CharacterUI> playerClones)
    {
        enemyClones = new();
        playerClones = new();

        foreach (var ui in CharacterUIManager.Instance.enemyUIs)
        {
            if (ui == null || ui.character == null) continue;

            Character clonedCh = (Character)ui.character.Clone();
            var go = new GameObject("simEnemyUI");
            if (simRoot != null) go.transform.SetParent(simRoot);  //삭제하기 편하게

            var cloneUI = go.AddComponent<CharacterUI>();
            cloneUI.character = clonedCh;

            enemyClones.Add(cloneUI);
        }

        foreach (var ui in CharacterUIManager.Instance.playerUIs)
        {
            if (ui == null || ui.character == null) continue;

            Character clonedCh = (Character)ui.character.Clone();
            var go = new GameObject("simPlayerUI");
            if (simRoot != null) go.transform.SetParent(simRoot);

            var cloneUI = go.AddComponent<CharacterUI>();
            cloneUI.character = clonedCh;

            playerClones.Add(cloneUI);
        }
    }

    //카드효과 처리(상대적 기준)
    private void ProcessCardWithSim(CardData card, List<CharacterUI> simPlayerUIS, List<CharacterUI> simEnemyUIs)
    {
        var processor = new CardProcessor();

        bool isDamageCard = card.effectMap.ContainsKey("Damage");

        if (isDamageCard)
        {
            foreach (var caster in simPlayerUIS)
            {
                var targets = GetSimTargets(card, simPlayerUIS, simEnemyUIs);
                processor.ProcessCard(card, caster, targets);
            }
        }
        else
        {
            var caster = simPlayerUIS[0];
            var targets = GetSimTargets(card, simPlayerUIS, simEnemyUIs);
            processor.ProcessCard(card, caster, targets);
        }
    }

    //에너미 기준 자기가 Player 상대적
    private List<CharacterUI> GetSimTargets(CardData card, List<CharacterUI> simPlayerUIs, List<CharacterUI> simEnemyUIs)
    {
        return card.cardTarget switch
        {
            CardTarget.oneEnemy => new List<CharacterUI> { simEnemyUIs.Last() },
            CardTarget.onePlayer => new List<CharacterUI> { simPlayerUIs.Last() },
            CardTarget.allEnemy => new List<CharacterUI>(simEnemyUIs),
            CardTarget.allPlayer => new List<CharacterUI>(simPlayerUIs),
            _ => new List<CharacterUI>()

        };
    }
    
    //리스트를 입력받으면 순열을 만듬
    private IEnumerable<List<T>> GetPermutations<T>(List<T> list)
    {
        if (list.Count == 0)
        {
            yield break;
        }

        foreach (var p in Permute(list, 0, list.Count - 1))
            yield return p;
        

        IEnumerable<List<T>> Permute(List<T> src, int l, int r)
        {
            if (l == r)
            {
                yield return new List<T>(src.Take(r + 1));
            }
            else
            {
                for (int i = l; i <= r; i++)
                {
                    (src[l], src[i]) = (src[i], src[l]);
                    foreach (var p in Permute(src, l + 1, r))
                        yield return p;
                    (src[l], src[i]) = (src[i], src[l]); // backtrack
                }
            }
        }
    }


}
