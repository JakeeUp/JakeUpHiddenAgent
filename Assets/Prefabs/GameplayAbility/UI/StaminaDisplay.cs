using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StaminaDisplay : MonoBehaviour
{
    [SerializeField] AbilityComponent abiltyComponet;
    PlayerValueGuage valueGuage;
    private void Start()
    {
        GameObject playerGameObject = GameplayStatics.GetPlayerGameObject();
        AbilityComponent abilityComponent = playerGameObject.GetComponent<AbilityComponent>();
        abilityComponent.onStaminaChanged += StaminaChange;
        valueGuage = GetComponent<PlayerValueGuage>();
        valueGuage.SetValue(abilityComponent.GetStamina(), abilityComponent.GetMaxStamina());

    }

    private void StaminaChange(float currentValue, float maxValue)
    {
        valueGuage.SetValue(currentValue, maxValue);
    }
}
