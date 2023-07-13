using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Item", menuName = "Inventory/Item")]
public class Item : ScriptableObject {
    public int itemId;
    public string itemName;
    public string itemDescription;
    public bool isStackable;
    public int itemValue;
    public int itemRarity;
    public Sprite itemIcon;
}

[CreateAssetMenu(fileName = "New Weapon", menuName = "Inventory/Weapon")]
public class Weapon : Item {
    public int damage;
    public int fireDamage;
}