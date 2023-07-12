using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class CustomItem {
    public Item item;
    public int itemCount;

    public CustomItem(Item item, int count) {
        this.item = item;
        itemCount = count;
    }
}


public class InventoryManager : MonoBehaviour {

    public static InventoryManager instance;
    
    public List<CustomItem> items = new List<CustomItem>();


    public Transform itemContent;
    public GameObject inventorySlot;
    public GameObject inventoryItem;
    public GameObject inventoryUI;

    void Start(){
        ShowInventory(false);
    }

    private void Awake() {
        if (instance != null) {
            Debug.LogWarning("More than one instance of InventoryManager found!");
            return;
        }

        instance = this;
    }
    public void Add(Item item, int count = 1) {
        CustomItem existingItem = items.Find(customItem => customItem.item == item);
        if (existingItem != null && item.isStackable) {
            existingItem.itemCount += count;
        }
        else {
            CustomItem newCustomItem = new CustomItem(item, count);
            items.Add(newCustomItem);
        }
    }

    public void Remove(Item item, int count = 1) {
        CustomItem existingItem = items.Find(customItem => customItem.item == item);
        if (existingItem != null) {
            existingItem.itemCount -= count;

            if (existingItem.itemCount <= 0) {
                items.Remove(existingItem);
            }
        }
    }

    public void AddToInventorySlots() {
        foreach (CustomItem customItem in items) {
            GameObject slot = Instantiate(inventorySlot, itemContent);
            GameObject item = Instantiate(inventoryItem, slot.transform);

            Text itemCountText = item.transform.Find("ItemCount").GetComponent<Text>();
            Image itemImage = item.transform.GetComponent<Image>();

            itemCountText.text = customItem.itemCount.ToString();
            itemImage.sprite = customItem.item.itemIcon;
        }
    }

    public void ClearInventorySlots() {
        foreach (Transform slot in itemContent) {
            Destroy(slot.gameObject);
        }
    }

    public void SortItemsByRarity(bool ascending) {
        List<CustomItem> sortedItems = new List<CustomItem>(items);

        if (ascending) {
            sortedItems.Sort((x, y) => x.item.itemRarity.CompareTo(y.item.itemRarity));
        } else {
            sortedItems.Sort((x, y) => y.item.itemRarity.CompareTo(x.item.itemRarity));
        }

        foreach (CustomItem customItem in sortedItems) {
            Debug.Log(customItem.item.itemName + " (Rarity: " + customItem.item.itemRarity + "): " + customItem.itemCount);
        }
    }

    public void ShowInventory(bool show) {
        if (show) {
            inventoryUI.SetActive(true);
            ClearInventorySlots();
            AddToInventorySlots();
            AddInventorySlots(10);
            SortItemsByRarity(false);
        }
        else {
            inventoryUI.SetActive(false);
        }
    }

    public void AddInventorySlots(int slotCount) {
        for (int i = 0; i < slotCount; i++) {
            GameObject slot = Instantiate(inventorySlot, itemContent);
            // GameObject item = Instantiate(inventoryItem, slot.transform);

            // Text itemCountText = item.transform.Find("ItemCount").GetComponent<Text>();
            // itemCountText.text = "JD";
        }
    }

    // public void Add(Item item) {
    //     if (items.ContainsKey(item)) {
    //         items[item] += 1;
    //     } else {
    //         items[item] = 1;
    //     }
    // }

    // public void Remove(Item item, int count) {
    //     if (items.ContainsKey(item)) {
    //         items[item] -= count;

    //         if (items[item] <= 0) {
    //             items.Remove(item);
    //         }
    //     }
    // }

    // public void ListItems() {
    //     foreach (KeyValuePair<Item, int> pair in items) {
    //         Debug.Log(pair.Key.itemName + ": " + pair.Value);
    //     }
    // }

    // public void SortItemsByRarity(bool ascending) {
    //     List<KeyValuePair<Item, int>> sortedItems = new List<KeyValuePair<Item, int>>(items);

    //     if (ascending) {
    //         sortedItems.Sort((x, y) => x.Key.itemRarity.CompareTo(y.Key.itemRarity));
    //     } else {
    //         sortedItems.Sort((x, y) => y.Key.itemRarity.CompareTo(x.Key.itemRarity));
    //     }

    //     foreach (KeyValuePair<Item, int> pair in sortedItems) {
    //         Debug.Log(pair.Key.name + " (Rarity: " + pair.Key.itemRarity + "): " + pair.Value);
    //     }
    // }
}