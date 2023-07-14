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
}