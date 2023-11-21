using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ShopItemWidget : MonoBehaviour
{
    [SerializeField] Button button;
    [SerializeField] Image icon;
    [SerializeField] TextMeshProUGUI itemName;
    [SerializeField] TextMeshProUGUI description;
    [SerializeField] TextMeshProUGUI price;
    [SerializeField] Color grayoutColor;
    [SerializeField] Color normalColor;

    ShopItem item;
    [SerializeField] Image crossedOut;
    internal void Init(ShopItem item, int credits, ShopUI buy)
    {
        icon.sprite = item.Icon;
        itemName.text = item.Title;
        price.text = "$" + item.Price.ToString();
        description.text = item.Description;
        this.item = item;

        button.onClick.AddListener(() => buy.SelectItem(item, gameObject));




        crossedOut.enabled = false;
        Refresh(credits);

    }

    public void Refresh(int credits)
    {
        if (credits < item.Price)
        {
            crossedOut.enabled = true;
            price.color = grayoutColor;
        }
        else
        {
            crossedOut.enabled = false;
            price.color = normalColor;
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
