using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField] CanvasGroup gameplayControl;
    [SerializeField] CanvasGroup gameplayMenu;

    void SetCanvasGroupEnable(CanvasGroup group,bool enabled)
    {
        group.blocksRaycasts = enabled;
        group.interactable = enabled;
    }

    public void SetGameplayControlEnabled(bool enabled)
    {
        SetCanvasGroupEnable (gameplayControl, enabled);
    }

    public void SetGameplayMenuEnabled(bool enabled)
    {
        SetCanvasGroupEnable(gameplayMenu, enabled);
    }
}
