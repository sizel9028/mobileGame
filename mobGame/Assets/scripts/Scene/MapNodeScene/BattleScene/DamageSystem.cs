using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 외부에서 직접 호출하여 데미지 이펙트 처리만 수행하는 시스템
/// 체력 감소 기능은 아직 구현 안 됨
/// </summary>
public class DamageSystem : MonoBehaviour
{
    [SerializeField] private GameObject damageVFX;

    /// <summary>
    /// 외부에서 이 함수를 호출해 데미지 처리 코루틴을 시작
    /// </summary>
    /// <param name="targets">이펙트를 줄 대상</param>
    public void StartDamageEffect(List<Transform> targets)
    {
        StartCoroutine(DamageEffectCoroutine(targets));
    }

    /// <summary>
    /// 대상에게 데미지 이펙트를 보여주는 코루틴
    /// 실제 체력 감소는 아직 구현 X
    /// </summary>
    private IEnumerator DamageEffectCoroutine(List<Transform> targets)
    {
        foreach (var target in targets)
        {
            // 데미지 이펙트 생성 (체력 감소는 아직 X)
            Instantiate(damageVFX, target.position, Quaternion.identity);
            yield return new WaitForSeconds(0.15f); // 간단한 연출 간격
        }
    }
}
