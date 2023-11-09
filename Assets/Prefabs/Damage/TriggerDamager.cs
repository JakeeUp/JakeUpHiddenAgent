using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerDamager : DamageComponent
{
    [SerializeField] float damage;
    [SerializeField] Collider trigger;
    private void Start()
    {
        trigger.enabled = false;
    }
    public override void DoDamage()
    {
        StartCoroutine(DamageCoroutine());
    }

    IEnumerator DamageCoroutine()
    {
        trigger.enabled = true;
        yield return new WaitForFixedUpdate();
        trigger.enabled = false;
    }
    protected override void ApplyDamage(GameObject target, float damage, GameObject instigator)
    {
        HealthComponet healthComponet = target.GetComponent<HealthComponet>();
        if (healthComponet != null)
        {
            healthComponet.ChangeHealth(-damage, instigator);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        DamageTarget(other.gameObject, damage, gameObject);
    }
}
