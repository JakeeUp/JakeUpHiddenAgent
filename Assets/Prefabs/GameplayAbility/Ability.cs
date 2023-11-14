using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public abstract class Ability : ScriptableObject
{
    [SerializeField] float staminaCost;
    [SerializeField] float cooldownDuration;

    bool onCooldown = false;
    public AbilityComponent OwningAbilityComponent
    {
        get;
        private set;
    }
    internal void Init(AbilityComponent abilityComponent)
    {
        OwningAbilityComponent = abilityComponent;
    }

    public bool CommitAbility() //return true if ability is able to be casted //if return true == also commit it, meaning stamina will be consumed and cooldown will start
    {
        if(onCooldown)
            return false;

        if(!OwningAbilityComponent.TryConsumeStamina(staminaCost))
            return false;

        StartCooldown();

        return true;
    }

    void StartCooldown()
    {
        StartCoroutine(CooldownCoroutine());

       
    }
    public Coroutine StartCoroutine(IEnumerator enumerator)
    {
        return OwningAbilityComponent.StartCoroutine(enumerator);
    }
    IEnumerator CooldownCoroutine()
    {
        onCooldown = true;
        yield return new WaitForSeconds(cooldownDuration);
        onCooldown = false;
    }

}
