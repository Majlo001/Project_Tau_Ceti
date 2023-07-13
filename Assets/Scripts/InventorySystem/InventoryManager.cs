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

    public int inventorySlotCount = 10;

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

    public void ReturnToInvenotry(CustomItem item) {
        items.Add(item);
        RefreshInventory();
    }

    public void Remove(CustomItem item) {
        if (item != null) {
            items.Remove(item);
        }
        RefreshInventory();
    }

    public void RemoveByCount(CustomItem item, int count = 1) {
        if (item != null) {
            item.itemCount -= count;

            if (item.itemCount <= 0) {
                items.Remove(item);
            }
        }
        RefreshInventory();
    }

    public void AddToInventorySlots() {
        foreach (CustomItem customItem in items) {
            GameObject slot = Instantiate(inventorySlot, itemContent);
            GameObject item = Instantiate(inventoryItem, slot.transform);

            item.GetComponent<DraggableItem>().item = customItem;
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
            RefreshInventory();
            // SortItemsByRarity(false);
        }
        else {
            inventoryUI.SetActive(false);
        }
    }

    private void RefreshInventory() {
        ClearInventorySlots();
        // SortItemsByRarity(false);
        AddToInventorySlots();
        AddInventorySlots(inventorySlotCount - items.Count);
    }

    public void AddInventorySlots(int slotCount) {
        for (int i = 0; i < slotCount; i++) {
            GameObject slot = Instantiate(inventorySlot, itemContent);
            // GameObject item = Instantiate(inventoryItem, slot.transform);

            // Text itemCountText = item.transform.Find("ItemCount").GetComponent<Text>();
            // itemCountText.text = "JD";
        }
    }
}