using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Helmet", menuName = "Inventory/Helmet")]
public class Helmet : Item {
    public int protection;
    public int fireResistance;
    
    public Helmet() {
        itemType = 2;
    }

    public override string GetTooltipStats() {
        string baseText = base.GetTooltipStats();

        string text = $"{baseText}\n" +
                        $"Protection: {protection}\n" +
                        $"Fire Resistance: {fireResistance}";

        return text;
    }
}