using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Item", menuName = "Inventory/Item")]
public class Item : ScriptableObject {
    public int itemId;
    public int itemType;
    public string itemName;
    public string itemDescription;
    public bool isStackable;
    public int itemValue;
    public int itemRarity;
    public Sprite itemIcon;
}