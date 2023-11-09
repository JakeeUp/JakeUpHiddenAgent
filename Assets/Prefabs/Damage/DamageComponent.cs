using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class DamageComponent : MonoBehaviour
{
    [SerializeField] bool damageFriendly;
    [SerializeField] bool damageHostile = true;
    [SerializeField] bool damageNeutral;

    ITeamInterface teamInterface;
    private void Awake()
    {
        teamInterface = GetComponent<ITeamInterface>();
    }

    public void SetTeamInterface(ITeamInterface teamInterface)
    {
        this.teamInterface = teamInterface;
    }

    
    public void DamageTarget(GameObject target, float damage, GameObject instigator)//have the target doing the damage
    {
        if (ShouldDamage(target))
        {
            ApplyDamage(target, damage, instigator);
        }

    }

   
    public virtual void DoDamage() //figure out target yourself
    {

    }
    protected abstract void ApplyDamage(GameObject target, float damage, GameObject instigator);

    bool ShouldDamage(GameObject target)
    {
        TeamRelation relation = teamInterface.GetRelationTowards(target);
        switch (relation)
        {
            case TeamRelation.Friendly:
                return damageFriendly;
            case TeamRelation.Hostile:
                return damageHostile;
            case TeamRelation.Neutral:
                return damageNeutral;
        }
        return false;
    }
}
