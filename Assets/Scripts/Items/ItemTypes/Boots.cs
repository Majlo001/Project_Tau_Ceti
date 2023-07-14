using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Boots", menuName = "Inventory/Boots")]
public class Boots : Item {
    public int protection;
    public int fireResistance;
    public int agility;
    
    public Boots() {
        itemType = 1;
    }
    
    public override string GetTooltipStats() {
        string baseText = base.GetTooltipStats();

        string text = $"{baseText}\n" +
                        $"Protection: {protection}\n" +
                        $"Fire Resistance: {fireResistance}\n" +
                        $"Agility: {agility}";

        return text;
    }
}
