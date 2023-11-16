using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerValueGuage : MonoBehaviour
{
    [SerializeField] Image fill;
    [SerializeField] TextMeshProUGUI text;

    public void SetValue(float current, float max)
    {
        fill.fillAmount = current/max;  
        text.text = ((int)current).ToString() + "/" + ((int)max).ToString();
    }
}
