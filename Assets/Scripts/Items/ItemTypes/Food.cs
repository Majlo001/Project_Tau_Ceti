using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Food", menuName = "Inventory/Consumables/Food")]
public class Food : Consumables {

    public Food() {
        itemType = 10;
        isStackable = true;
    }

    // public override string GetTooltipStats() {
    //     string baseText = base.GetTooltipStats();

    //     string text = $"{baseText}\n" +
    //                     $"Healing: {healingPoints}";

    //     return text;
    // }
}