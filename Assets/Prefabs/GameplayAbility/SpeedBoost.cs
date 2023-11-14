using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Ability/Speed Boost")]
public class SpeedBoost : Ability
{
    [SerializeField] float boostAmt;
    [SerializeField] float boostDuration;

    IMovementInterface movementInterface;
    public override void ActivateAbility()
    {
        if (!CommitAbility()) return;
        movementInterface = OwningAbilityComponent.GetComponent<IMovementInterface>();
        if(movementInterface != null )
        {
            movementInterface.SetMoveSpeed(movementInterface.GetMoveSpeed() + boostAmt);
            StartCoroutine(ResetSpeed());
        }
    }

    IEnumerator ResetSpeed()
    {
        yield return new WaitForSeconds(boostDuration);
        movementInterface.SetMoveSpeed(movementInterface.GetMoveSpeed() - boostAmt);
    }
}
