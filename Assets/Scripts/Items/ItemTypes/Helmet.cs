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
}