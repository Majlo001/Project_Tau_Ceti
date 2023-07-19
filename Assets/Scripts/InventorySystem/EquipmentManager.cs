using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EquipmentManager : MonoBehaviour {
    
    public static EquipmentManager instance;

    public GameObject equipmentUI;
    public GameObject weaponSlot;
    public GameObject helmetSlot;
    public GameObject chestplateSlot;
    public GameObject bootsSlot;

    public int equipmentSlotCount = 4;
    public GameObject[] consumableSlots = new GameObject[4];
    public CustomItem[] equippedConsumables = new CustomItem[4];


    private Dictionary<string, CustomItem> equippedItems = new Dictionary<string, CustomItem>();
    private Dictionary<string, GameObject> slotObjects = new Dictionary<string, GameObject>();


    void Awake() {
        if (instance != null) {
            Debug.LogWarning("More than one instance of EquipmentManager found!");
            return;
        }

        instance = this;

        equippedItems.Add("Weapon", null);
        equippedItems.Add("Helmet", null);
        equippedItems.Add("Chestplate", null);
        equippedItems.Add("Boots", null);

        slotObjects.Add("Weapon", weaponSlot);
        slotObjects.Add("Helmet", helmetSlot);
        slotObjects.Add("Chestplate", chestplateSlot);
        slotObjects.Add("Boots", bootsSlot);
    }

    public void EquipItem(CustomItem item, string slotType) {
        if (equippedItems.ContainsKey(slotType)) {
            UnequipItem(slotType);
        }

        equippedItems[slotType] = item;
    }

    public void UnequipItem(string slotType) {
        equippedItems[slotType] = null;
    }

    
    public void EquipConsumable(CustomItem item, int slotNumber) {
        if (equippedConsumables[slotNumber] != null) {
            UnequipConsumable(slotNumber);
        }

        equippedConsumables[slotNumber] = item;
        PrintConsumables();
    }

    public void UnequipConsumable(int slotNumber) {
        equippedConsumables[slotNumber] = null;
    }

    public void SwapConsumable(int slotNumber1, int slotNumber2) {
        CustomItem tempItem = equippedConsumables[slotNumber1];
        
        equippedConsumables[slotNumber1] = equippedConsumables[slotNumber2];
        equippedConsumables[slotNumber2] = tempItem;
        PrintConsumables();
    }

    public bool AddToEquippedConsumable(Item item, int count) {
        for(int i=0; i<equippedConsumables.Length; i++) {
            if (equippedConsumables[i] != null) {
                if (equippedConsumables[i].item == item) {
                    equippedConsumables[i].itemCount += count;
                    UpdateItemCountText(i);
                    
                    return true;
                }
            }
        }

        return false;
    }

    private void UpdateItemCountText(int i) {
        consumableSlots[i].GetComponentInChildren<Text>().text = consumableSlots[i].GetComponent<ConsumableSlot>().item.itemCount.ToString();
    }


    public void PrintConsumables() {
        string temp = "";
        string temptext;

        for(int i=0; i<equipmentSlotCount; i++){
            if (consumableSlots[i].GetComponent<ConsumableSlot>().item != null) {
                temptext = consumableSlots[i].GetComponent<ConsumableSlot>().item.item.itemName;
            } else {
                temptext = "null";
            }
                

            if (equippedConsumables[i] != null) {
                temp += i + ": " + equippedConsumables[i].item.itemName + " " + temptext +"\n";
            } else {
                temp += i + ": " + "null" + " " + temptext +"\n";
            }
        }
        Debug.Log(temp);
    }

    public void UpdateEquippedConsumables() {
        for (int i = 0; i < equipmentSlotCount; i++) {
            if (equippedConsumables[i] != null) {
                if (consumableSlots[i].transform.childCount == 0) {
                    equippedConsumables[i] = null;
                    consumableSlots[i].GetComponent<ConsumableSlot>().item = null;
                }
            }
        }
    }
}
