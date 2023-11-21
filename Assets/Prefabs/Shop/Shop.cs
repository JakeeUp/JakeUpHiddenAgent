using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Shop/Shop" )]
public class Shop : ScriptableObject
{
    [SerializeField] ShopItem[] items;

    public bool TryPurchase(ShopItem item, CreditComponent buyer)
    {
        return buyer.TryPurchaseItem(item);
    }

}
