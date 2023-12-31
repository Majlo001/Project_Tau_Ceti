using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EquipmentManager : MonoBehaviour {
    
    public static EquipmentManager instance;
    private ConsumablesHotbarController consumablesHotbarController;

    public GameObject equipmentUI;
    public GameObject weaponSlot;
    public GameObject helmetSlot;
    public GameObject chestplateSlot;
    public GameObject bootsSlot;

    private int consumablesSlotCount;
    private int equipmentSlotCount;
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
        consumablesHotbarController = transform.GetComponent<ConsumablesHotbarController>();

        equippedItems.Add("Weapon", null);
        equippedItems.Add("Helmet", null);
        equippedItems.Add("Chestplate", null);
        equippedItems.Add("Boots", null);

        slotObjects.Add("Weapon", weaponSlot);
        slotObjects.Add("Helmet", helmetSlot);
        slotObjects.Add("Chestplate", chestplateSlot);
        slotObjects.Add("Boots", bootsSlot);

        consumablesSlotCount = consumableSlots.Length;
        equipmentSlotCount = equippedItems.Count;
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

    public CustomItem takeItem(string slotType) {
        return equippedItems[slotType];
    }

    
    public void EquipConsumable(CustomItem item, int slotNumber) {
        if (equippedConsumables[slotNumber] != null) {
            UnequipConsumable(slotNumber);
        }

        equippedConsumables[slotNumber] = item;
        //PrintConsumables();
        consumablesHotbarController.UpdateHotbarItems();
    }

    public void UnequipConsumable(int slotNumber) {
        equippedConsumables[slotNumber] = null;
        consumablesHotbarController.UpdateHotbarItems();
    }

    public void SwapConsumable(int slotNumber1, int slotNumber2) {
        CustomItem tempItem = equippedConsumables[slotNumber1];
        
        equippedConsumables[slotNumber1] = equippedConsumables[slotNumber2];
        equippedConsumables[slotNumber2] = tempItem;
        //PrintConsumables();
        consumablesHotbarController.UpdateHotbarItems();
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
        consumableSlots[i].transform.GetChild(0).GetComponentInChildren<Text>().text = equippedConsumables[i].itemCount.ToString();
        consumablesHotbarController.UpdateHotbarItemCountText(i);
    }

    public void UseConsumable(int i) {
        if (equippedConsumables[i] != null) {
            if (equippedConsumables[i].itemCount > 0) {
                equippedConsumables[i].itemCount--;
                UpdateItemCountText(i);

                // TODO: Use Item
                // equippedConsumables[i].item.Use();
            }

            if (equippedConsumables[i].itemCount == 0) {
                // TODO: Is Item Destroyed?
                ConsumableSlot consumableSlot = consumableSlots[i].transform.GetChild(0).GetComponent<ConsumableSlot>();
                consumableSlot.RemoveItem();
                consumableSlot.RemoveDraggableItem();
            }
        }
    }


    public void PrintEquipment() {
        string temp = "";
        string temptext;

        foreach (KeyValuePair<string, CustomItem> equippedItem in equippedItems) {
            
            temptext = "null";
            if (slotObjects.TryGetValue(equippedItem.Key, out GameObject slot)) {
                if (slot.transform.GetChild(0).GetComponent<EquipmentSlot>().item != null) {
                    temptext = slot.transform.GetChild(0).GetComponent<EquipmentSlot>().item.item.itemName;
                }
            }
                

            if (equippedItem.Value != null) {
                temp += equippedItem.Key + ": " + equippedItem.Value.item.itemName + " " + temptext +"\n";
            } else {
                temp += equippedItem.Key + ": " + "null" + " " + temptext +"\n";
            }
        }
        Debug.Log(temp);
    }

    public void PrintConsumables() {
        //PrintEquipment();
        string temp = "";
        string temptext;

        for(int i=0; i < consumablesSlotCount; i++){
            if (consumableSlots[i].transform.GetChild(0).GetComponent<ConsumableSlot>().item != null) {
                temptext = consumableSlots[i].transform.GetChild(0).GetComponent<ConsumableSlot>().item.item.itemName;
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
        for (int i = 0; i < consumablesSlotCount; i++) {
            if (equippedConsumables[i] != null) {
                if (consumableSlots[i].transform.childCount == 0) {
                    equippedConsumables[i] = null;
                    consumableSlots[i].GetComponent<ConsumableSlot>().item = null;
                }
            }
        }
    }
}
