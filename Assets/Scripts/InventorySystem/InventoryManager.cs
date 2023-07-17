using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;


public class CustomItem {
    public Item item;
    public int itemCount;
    public int itemLevel;
    public bool isUpgraded;

    public CustomItem(Item item, int count, int level = 0) {
        this.item = item;
        itemCount = count;
        isUpgraded = false;

        if (level == 0) {
            itemLevel = item.itemLevel;
        }
        else {
            itemLevel = level;
        }
    }
}


public class InventoryManager : MonoBehaviour {

    public static InventoryManager instance;
    
    public List<CustomItem> items = new List<CustomItem>();

    public Transform itemContent;
    public GameObject inventorySlot;
    public GameObject inventoryItem;
    public GameObject inventoryUI;
    private EquipmentManager equipmentManager;

    public int inventorySlotCount = 10;
    private bool showAllTypes = true;
    private int[] currentItemTypes;

    void Start(){
        ShowInventory(false);
        equipmentManager = transform.GetComponent<EquipmentManager>();
    }

    private void Awake() {
        if (instance != null) {
            Debug.LogWarning("More than one instance of InventoryManager found!");
            return;
        }

        instance = this;
    }
    public void Add(Item item, int count = 1) {
        if (item == null) {
            return;
        }

        if (ItemTypeDictionary.Instance.itemTypeConsumables.Contains(item.itemType)) {
            bool isEquippedConsumable = equipmentManager.AddToEquippedConsumable(item, count);

            if (isEquippedConsumable) {
                return;
            }
        }

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
            Image itemImage = item.transform.Find("ItemImage").GetComponent<Image>();
            
            if (customItem.item.isStackable && customItem.itemCount > 1) {
                itemCountText.text = customItem.itemCount.ToString();
            }
            else {
                itemCountText.text = "";
            }
            itemImage.sprite = customItem.item.itemIcon;


            //TODO: Change outline to image or sth.
            // Outline itemOverlay = item.transform.Find("ItemOverlay").GetComponent<Outline>();
            
            // itemOverlay.effectColor = Color.red;
            // itemOverlay.effectDistance = new Vector2(2f, 2f);

        }
    }

    public void ClearInventorySlots() {
        foreach (Transform slot in itemContent) {
            Destroy(slot.gameObject);
        }
    }

    // TODO: Check if it works
    public void SortItemsByRarity(bool ascending) {
        List<CustomItem> sortedItems = new List<CustomItem>(items);

        if (ascending) {
            sortedItems.Sort((x, y) => x.item.itemRarity.CompareTo(y.item.itemRarity));
        } else {
            sortedItems.Sort((x, y) => y.item.itemRarity.CompareTo(x.item.itemRarity));
        }

        items = sortedItems;
        sortedItems.Clear();
        sortedItems = null;
    }

    public void ShowInventory(bool show) {
        if (show) {
            showAllTypes = true;
            currentItemTypes = null;
            inventoryUI.SetActive(true);
            // SortItemsByRarity(false);
            RefreshInventory();
        }
        else {
            inventoryUI.SetActive(false);
            TooltipSystem.Hide();
        }
    }

    private void RefreshInventory() {
        if (showAllTypes) {
            ClearInventorySlots();
            // SortItemsByRarity(false);
            AddToInventorySlots();
            AddInventorySlots(inventorySlotCount - items.Count);
        } 
        else {
            ShowItemsByType(currentItemTypes);
        }
    }

    public void AddInventorySlots(int slotCount) {
        for (int i = 0; i < slotCount; i++) {
            GameObject slot = Instantiate(inventorySlot, itemContent);
        }
    }

    public void showAllItemTypes() {
        showAllTypes = true;
        currentItemTypes = null;
        RefreshInventory();
    }

    public void ShowItemsByType(int[] itemTypes) {
        showAllTypes = false;
        currentItemTypes = itemTypes;

        ClearInventorySlots();

        foreach (CustomItem customItem in items) {
            if (itemTypes.Any(itemType => itemType == customItem.item.itemType)) {
                GameObject slot = Instantiate(inventorySlot, itemContent);
                GameObject item = Instantiate(inventoryItem, slot.transform);

                item.GetComponent<DraggableItem>().item = customItem;
                Text itemCountText = item.transform.Find("ItemCount").GetComponent<Text>();
                Image itemImage = item.transform.Find("ItemImage").GetComponent<Image>();

                if (customItem.item.isStackable && customItem.itemCount > 1) {
                    itemCountText.text = customItem.itemCount.ToString();
                }
                else {
                    itemCountText.text = "";
                }
                itemImage.sprite = customItem.item.itemIcon;
            }
        }

        AddInventorySlots(50);
    }
}