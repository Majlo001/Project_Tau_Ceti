using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Reflection;

public struct StatsData {
    public string statName;
    public string fieldName;
    public object statValue;

    public StatsData(string name, string field, object value) {
        statName = name;
        fieldName = field;
        statValue = value;
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
        string colorHex = ItemRarityDictionary.Instance.itemColors[itemRarity];

        string itemRarityText = ItemRarityDictionary.Instance.itemRarity[itemRarity];
        string text = $"<color={colorHex}>{itemRarityText}</color>";

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
}


public class Stats {
    /*
    *  Empty class used to create a variables of type Stats.
    */

    public virtual StatsData[] GetStatsFields() {
        Debug.Log("StatsData in Stats class");
        return null;
    }
};