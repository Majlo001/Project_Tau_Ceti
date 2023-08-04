using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Reflection;

public struct StatsData {
    public string fieldName;
    public string statName;
    public object statValue;

    public StatsData(string field, string name, object value) {
        fieldName = field;
        statName = name;
        statValue = value;
    }

    public object GetStatValue(string fieldName) {
        if (fieldName != this.fieldName)
            return null;
        
        return statValue;
    }
}

[CreateAssetMenu(fileName = "New Item", menuName = "Inventory/Item")]
public class Item : ScriptableObject {
    public int itemId;
    public int itemLevel;
    public int itemType;
    public string itemName;
    public string itemDescription;
    public bool isStackable;
    public int itemValue;
    public int itemRarity;
    public Sprite itemIcon;
    public Stats itemStats = new Stats();

    public virtual string GetTooltipStats() {
        string text = $"Type: {itemType}\n" + 
                        $"Level: {itemLevel}";

        return text;
    }
    public string GetTooltipRarityText() {
        string itemTypeString = ItemTypeDictionary.Instance.itemType[itemType];
        string colorHex = ItemRarityDictionary.Instance.itemColors[itemRarity];

        string itemRarityText = ItemRarityDictionary.Instance.itemRarity[itemRarity];
        string text = $"<color={colorHex}>{itemRarityText} {itemTypeString}</color>";

        return text;
    }

    public int GetItemRarity() {
        return itemRarity;
    }

    public float GetItemVaule() {
        return itemValue;
    }
    
    public Stats GetStats() {
        return itemStats;
    }

    public virtual string GetRangeText() {
        return null;
    }

    public virtual int[] GetRangeTab() {
        return null;
    }

    public virtual int getRangeIndex(int i) {
        return 0;
    }
}


public class Stats {
    /*
    *  Empty class used to create a variables of type Stats.
    */

    public virtual StatsData[] GetStatsFields() {
        return null;
    }
};