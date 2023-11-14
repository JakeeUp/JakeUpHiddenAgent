using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AbilityWidget : MonoBehaviour
{
    Ability ability;
    [SerializeField] Image icon;
    [SerializeField] Image CooldownImage;

    [SerializeField] float highlightedScale = 1.5f;
    [SerializeField] float scaleSpeed = 20f;
    [SerializeField] float highlightOffset = 200f;

    [SerializeField] RectTransform widgetRoot;

    Vector3 goalScale = Vector3.one;
    Vector3 goalOffset = Vector3.zero;

    public void SetScaleAmount(float amt) //amt == 0 means not scaled up, amt == 1 means scaled to 1.5
    {
        goalScale = Vector3.one *(1 + amt * (highlightedScale - 1));
        goalOffset = Vector3.left * amt * highlightOffset;
    }

    
    internal void Init(Ability ability)
    {
        this.ability = ability;
        icon.sprite = ability.GetIcon();
        ability.onCooldownStarted += cooldownStarted;
        

    }

    
    private void cooldownStarted(float cooldownDuration)
    {
        StartCoroutine(cooldownTime(cooldownDuration));


    }
    IEnumerator cooldownTime(float cooldownDuration)
    {
        float coolDownTimeLeft = cooldownDuration;
        while(coolDownTimeLeft > 0)
        {
            coolDownTimeLeft -= Time.deltaTime;
            CooldownImage.fillAmount = coolDownTimeLeft/cooldownDuration;

            yield return new WaitForEndOfFrame();

        }
    }
    

    public void ActivateAbility()
    {
        ability.ActivateAbility();
        CooldownImage.fillAmount = 1;
    }

    private void Update()
    {
        widgetRoot.transform.localPosition = Vector3.Lerp(widgetRoot.transform.localPosition, goalOffset, Time.deltaTime * scaleSpeed);
        widgetRoot.transform.localScale = Vector3.Lerp(widgetRoot.transform.localScale, goalScale, Time.deltaTime * scaleSpeed);
    }



}
