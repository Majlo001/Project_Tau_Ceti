using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Item", menuName = "Inventory/Item")]
public class Item : ScriptableObject {
    public int itemId;
    public int itemLevel;
    public int itemType;
    public string itemName;
    public string itemDescription;
    public bool isStackable;
    public bool isConsumable = false;
    public int itemValue;
    public int itemRarity;
    public Sprite itemIcon;

    public virtual string GetTooltipStats() {
        string text = $"Value: {itemValue}\n" +
                        $"Type: {itemType}\n" + 
                        $"Level: {itemLevel}";

        return text;
    }
    public string GetTooltipRarity() {
        string colorHex = ItemRarityDictionary.Instance.itemColors[itemRarity];

        string itemRarityText = ItemRarityDictionary.Instance.itemRarity[itemRarity];
        string text = $"<color={colorHex}>{itemRarityText}</color>";

        return text;
    }
}