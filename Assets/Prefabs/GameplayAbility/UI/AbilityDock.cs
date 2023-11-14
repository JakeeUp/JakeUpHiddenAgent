using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class AbilityDock : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    [SerializeField] AbilityComponent owningAbilityComponent;
    [SerializeField] AbilityWidget abilityWidgetPrefab;
    [SerializeField] RectTransform widgetRoot;
    [SerializeField] float scaleRange = 100f;
    List<AbilityWidget> abilityWidgets = new List<AbilityWidget>();

    PointerEventData touchData;
    AbilityWidget highlightedAbilityWidget;

    private void Awake()
    {
        owningAbilityComponent.onNewAbilityAdded += AddNewAbilityWidget;
    }

    private void AddNewAbilityWidget(Ability ability)
    {
        AbilityWidget newWidget = Instantiate(abilityWidgetPrefab, widgetRoot);
        newWidget.Init(ability);
        abilityWidgets.Add(newWidget);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        touchData = eventData;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if(highlightedAbilityWidget != null)
        {
            highlightedAbilityWidget.ActivateAbility();
        }

        touchData = null;
    }

    private void Update()
    {
        if(touchData != null)
        {
            highlightedAbilityWidget = GetWidgetFromTouchData();
        }

        UpdateScale();
    }

    private void UpdateScale()
    {
        if(touchData == null)
        {
            abilityWidgets.ForEach(widget => { widget.SetScaleAmount(0); });
        }
        else
        {
            float touchPosY = touchData.position.y;
            foreach(AbilityWidget widget in abilityWidgets)
            {
                float widgetPosY = widget.transform.position.y;
                float distanceY = Mathf.Abs(widgetPosY - touchPosY); 
                if(distanceY > scaleRange)
                {
                    widget.SetScaleAmount(0);
                }
                else
                {
                    widget.SetScaleAmount((scaleRange - distanceY)/scaleRange);
                }
            }
        }

    }

    private AbilityWidget GetWidgetFromTouchData()
    {
        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(touchData, results);
        foreach(RaycastResult result in results)
        {
            AbilityWidget foundWidget = result.gameObject.GetComponent<AbilityWidget>();
            if(foundWidget != null)
            {
                return foundWidget; 
            }
        }
        return null;
    }
}
