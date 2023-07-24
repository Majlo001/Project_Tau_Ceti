using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Potion", menuName = "Inventory/Consumables/Potion")]
public class Potion : Consumables {

    public Potion() {
        itemType = 10;
        isStackable = true;
    }
}
