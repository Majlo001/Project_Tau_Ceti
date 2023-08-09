using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConsumablesHotbarController : MonoBehaviour {
    public GameObject[] consumablesHotbarSlots = new GameObject[4];
    public CustomItem[] consumablesHotbarItems = new CustomItem[4];

    private EquipmentManager equipmentManager;

    public void Start() {
        equipmentManager = GameObject.Find("InventoryManager").GetComponent<EquipmentManager>();
    }

    public void UpdateHotbarItems() {
        consumablesHotbarItems = equipmentManager.equippedConsumables;
    }

    public void RefreshItemsCount() {
        // for (int i = 0; i < consumablesHotbarSlots.Length; i++) {
        //     if (consumablesHotbarItems[i] != null) {
        //         consumablesHotbarSlots[i].GetComponent<ConsumablesHotbarSlot>().UpdateSlot(consumablesHotbarItems[i]);
        //     } else {
        //         consumablesHotbarSlots[i].GetComponent<ConsumablesHotbarSlot>().ClearSlot();
        //     }
        // }
    }

    // public void UpdateHotbarSlots() {
    //     for (int i = 0; i < consumablesHotbarSlots.Length; i++) {
    //         if (PlayerController.instance.consumablesHotbar[i] != null) {
    //             consumablesHotbarSlots[i].GetComponent<ConsumablesHotbarSlot>().UpdateSlot(PlayerController.instance.consumablesHotbar[i]);
    //         } else {
    //             consumablesHotbarSlots[i].GetComponent<ConsumablesHotbarSlot>().ClearSlot();
    //         }
    //     }
    // }
}
