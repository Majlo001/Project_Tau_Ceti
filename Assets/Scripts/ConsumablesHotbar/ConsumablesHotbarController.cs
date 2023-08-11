using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ConsumablesHotbarController : MonoBehaviour {

    public static ConsumablesHotbarController instance;
    
    private EquipmentManager equipmentManager;

    public GameObject[] consumablesHotbarSlots = new GameObject[4];
    public CustomItem[] consumablesHotbarItems = new CustomItem[4];

    
    private void Awake() {
        if (instance != null) {
            Debug.LogWarning("More than one instance of InventoryManager found!");
            return;
        }

        instance = this;
        equipmentManager = GameObject.Find("InventoryManager").GetComponent<EquipmentManager>();
    }

    public void UpdateHotbarItems() {
        for(int i=0; i<consumablesHotbarSlots.Length; i++) {
            if (equipmentManager.equippedConsumables[i] != consumablesHotbarItems[i]) {
                consumablesHotbarItems[i] = equipmentManager.equippedConsumables[i];

                if (consumablesHotbarItems[i] != null) {
                    consumablesHotbarSlots[i].transform.Find("ItemImage").GetComponent<Image>().sprite = consumablesHotbarItems[i].item.itemIcon;
                    UpdateHotbarItemCountText(i);
                } else {
                    consumablesHotbarSlots[i].transform.Find("ItemImage").GetComponent<Image>().sprite = null;
                    consumablesHotbarSlots[i].transform.Find("ItemName").GetComponent<Text>().text = "Empty slot";
                }
            }
        }
    }

    public void UpdateHotbarItemCountText(int i) {
        string itemNameText = consumablesHotbarItems[i].itemCount.ToString() + "x - " + consumablesHotbarItems[i].item.itemName;
        consumablesHotbarSlots[i].transform.Find("ItemName").GetComponent<Text>().text = itemNameText;
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
