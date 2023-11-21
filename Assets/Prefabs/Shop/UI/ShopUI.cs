using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopUI : MonoBehaviour
{
    [SerializeField] Shop shop;
    [SerializeField] ShopItemWidget itemWidgetPrefab;
    [SerializeField] Transform shopList;
    [SerializeField] BuyComponent buyComponent;
    [SerializeField] CreditComponent creditComponent;

    ShopItem shopitem;
    GameObject selectedGameObject;

    List<ShopItemWidget> shopItemWidgets = new List<ShopItemWidget>();

    private void Awake()
    {
        foreach(var item in shop.GetItems())
        {
            ShopItemWidget newWidget = Instantiate(itemWidgetPrefab, shopList);
            newWidget.Init(item, creditComponent.Credits, this);
        }

        creditComponent.onCreditChanged += RefreshItems;
    }

    private void RefreshItems(int newCredit)
    {
        foreach(ShopItemWidget item in shopItemWidgets)
        {
            item.Refresh(newCredit);
        }
    }

  
    public void SelectItem(ShopItem item, GameObject itemGameObject)
    {
        shopitem = item;
        selectedGameObject = itemGameObject;
    }

    public void BuyItem()
    {
        shop.TryPurchase(shopitem, creditComponent);
        Destroy(selectedGameObject);
    }
}
