using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Consumables : Item {
    int healingPoints;
    
    
    public Consumables() {
        itemType = 10;
        isStackable = true;
    }

    public override string GetTooltipStats() {
        string baseText = base.GetTooltipStats();

        string text = $"{baseText}\n" +
                        $"Healing: {healingPoints}";

        return text;
    }
}
