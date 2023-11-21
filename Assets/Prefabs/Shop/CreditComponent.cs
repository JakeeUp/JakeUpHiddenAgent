using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPurchaseListener
{
    bool ItemPurchased(Object newPurchase);
}
public class CreditComponent : MonoBehaviour
{
    [SerializeField] int credits;

    IPurchaseListener[] purchaseListeners;
    private void Awake()
    {
        purchaseListeners = GetComponents<IPurchaseListener>();
    }
    public int Credits { get { return credits; } }

    public delegate void OnCreditChanged(int newCredit);
    public event OnCreditChanged onCreditChanged;
    public bool TryPurchaseItem(ShopItem item)
    {
        if(credits < item.Price)
            return false;

        credits -= item.Price;
        onCreditChanged?.Invoke(credits);
        foreach(IPurchaseListener listener in purchaseListeners)
        {
            if(listener.ItemPurchased(item.Item))
                break;
        }
        return true;
           
    }
}
