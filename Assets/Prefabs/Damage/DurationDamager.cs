using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DurationDamager : MonoBehaviour
{
    float duration;
    float damage;

    public void Init(float duration, float damage, HealthComponet healthComp, GameObject instigator)
    {
        this.duration = duration;
        this.damage = damage;

        StartCoroutine(DamageCoroutine(healthComp, instigator));
    }

    IEnumerator DamageCoroutine(HealthComponet healthComp, GameObject instigator)
    {
        float timeElasped = 0;
        float damageRate = damage / duration;
        while (timeElasped < duration)
        {
            timeElasped += Time.deltaTime;
            healthComp.ChangeHealth(-damageRate * Time.deltaTime, instigator);
            yield return new WaitForEndOfFrame();

        }
        Destroy(this);
    }
   
}
