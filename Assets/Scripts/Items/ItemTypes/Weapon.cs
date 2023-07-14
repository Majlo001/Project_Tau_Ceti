using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Weapon", menuName = "Inventory/Weapon")]
public class Weapon : Item {
    public int damage;
    public int fireDamage;
    
    public Weapon() {
        itemType = 0;
    }

    public override string GetTooltipStats() {
        string baseText = base.GetTooltipStats();

        string text = $"{baseText}\n" +
                            $"Damage: {damage}\n" +
                            $"Fire Damage: {fireDamage}";

        return text;
    }
}