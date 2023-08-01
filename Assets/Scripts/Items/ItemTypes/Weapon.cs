using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Reflection;

[CreateAssetMenu(fileName = "New Weapon", menuName = "Inventory/Weapon")]
public class Weapon : Item {
    public int[] damageRange = new int[2];

    public new WeaponStats itemStats = new WeaponStats();
    
    public Weapon() {
        itemType = 0;
    }

    public override string GetTooltipStats() {
        string baseText = base.GetTooltipStats();

        string text = $"{baseText}\n";

        return text;
    }
}


[System.Serializable]
public class WeaponStats : Stats{
    public int fireDamage;
    public int posionDamage;
    public int iceDamage;
    public int lightningDamage;
    public int crushingDamage;
    public float attackSpeed;
    public int critChance;
    public int critDamage;
    

    public override StatsData[] GetStatsFields() {
        StatsData[] baseStats = base.GetStatsFields();
        
        Debug.Log("StatsData");
        Type type = this.GetType();
        FieldInfo[] fields = type.GetFields();

        StatsData[] statsTab = new StatsData[fields.Length];
        int it = 0;

        foreach (var field in fields) {
            string fieldName = field.Name;
            object fieldValue = field.GetValue(this);
            Debug.Log("fieldName: " + fieldName + " fieldValue: " + fieldValue);

            if (fieldValue != null) {
                if ((fieldValue is int intValue && intValue > 0) || (fieldValue is float floatValue && floatValue > 0f)) {
                    string statName = ItemTypeDictionary.Instance.weaponStatsDictionary[fieldName];
                    statsTab[it] = new StatsData(statName, fieldName, fieldValue);
                    it++;
                }
            }
        }

        Array.Resize(ref statsTab, it);
        return statsTab;
    }
}