using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(menuName = "Ability/Health Boost")]
public class HealthRegen : Ability
{
    [SerializeField] float healthAmt;
    [SerializeField] float healthRegenDuration;

    HealthComponet healthComponet;

  
    public override void ActivateAbility()
    {
        if (!CommitAbility())
            return;
        //if (healthComponet == null || healthComponet.isFull())
        //    return;
        healthComponet = OwningAbilityComponent.GetComponent<HealthComponet>();
        StartCoroutine(HealthRegenAbility());

    }

    IMovementInterface movementInterface;

    IEnumerator HealthRegenAbility()
    {

        float timeLeft = healthRegenDuration;
        while(timeLeft > 0)
        {
            timeLeft -= Time.deltaTime;
            float deltaTime = Time.deltaTime;
            if(timeLeft < 0)
            {
                deltaTime += timeLeft;
            }
            float healthRegen = (healthAmt * Time.deltaTime) / healthRegenDuration;
            healthComponet.ChangeHealth(healthRegen, OwningAbilityComponent.gameObject);
            yield return new WaitForEndOfFrame();

        }
        
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
