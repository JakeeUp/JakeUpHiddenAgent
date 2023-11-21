using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Shop/Item")]
public class ShopItem : ScriptableObject
{
 
    [SerializeField] string title;
    [SerializeField] int price;
    [SerializeField] Sprite icon;
    [SerializeField] Object item;
    [SerializeField, TextArea(10, 10)] string description;

    public string Title { get { return title; } }
    public int Price { get{ return price; } }
    public Sprite Icon { get { return icon; } }
    public Object Item { get { return item; } }
    public string Description { get { return description; } }

}

