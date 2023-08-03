using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;


public class CustomItemComparer : IComparer<CustomItem> {
    public int Compare(CustomItem x, CustomItem y) {
        if (x.item.itemLevel != y.item.itemLevel) {
            return y.item.itemLevel.CompareTo(x.item.itemLevel);
        }
        else if (x.item.itemRarity != y.item.itemRarity) {
            return y.item.itemRarity.CompareTo(x.item.itemRarity);
        }
        else {
            return x.item.itemName.CompareTo(y.item.itemName);
        }
    }
}


public class CustomItem {
    public Item item;
    public int itemCount;
    public int itemLevel;
    public bool isUpgraded;
    public Stats itemUpgradedStats;

    public Stats GetStats() {
        if (item is Weapon) {
            return ((Weapon)item).itemStats;
        }

        return null;
    }

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

    public int inventorySlotCount = 5;
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
    public bool AddItem(Item item, int count = 1) {
        if (item == null)
            return false;

        if (ItemTypeDictionary.Instance.itemTypeConsumables.Contains(item.itemType)) {
            bool isEquippedConsumable = equipmentManager.AddToEquippedConsumable(item, count);

            if (isEquippedConsumable)
                return true;
        }

        CustomItem existingItem = items.Find(customItem => customItem.item == item);
        if (existingItem != null && item.isStackable) {
            existingItem.itemCount += count;
        }
        else {
            if (inventorySlotCount <= items.Count) {
                Debug.Log("Inventory is full");
                return false;
            }

            CustomItem newCustomItem = new CustomItem(item, count);
            items.Add(newCustomItem);
            items.Add(newCustomItem);
            items.Add(newCustomItem);
            items.Add(newCustomItem);
            items.Add(newCustomItem);
        }
        return true;
    }
    public bool AddItem(CustomItem item) {
        if (item == null)
            return false;

        if (ItemTypeDictionary.Instance.itemTypeConsumables.Contains(item.item.itemType)) {
            bool isEquippedConsumable = equipmentManager.AddToEquippedConsumable(item.item, item.itemCount);

            if (isEquippedConsumable)
                return true;
        }

        CustomItem existingItem = items.Find(customItem => customItem.item == item.item);
        if (existingItem != null && item.item.isStackable) {
            existingItem.itemCount += item.itemCount;
        }
        else {
            if (inventorySlotCount <= items.Count) {
                Debug.Log("Inventory is full");
                return false;
            }

            items.Add(item);
            items.Add(item);
            items.Add(item);
            items.Add(item);
            items.Add(item);
        }
        return true;
    }

    public bool ReturnToInventory(CustomItem item, bool isSwap = false) {
        if (item == null)
            return false;

        if (isSwap != true && inventorySlotCount <= items.Count) {
            Debug.Log("Inventory is full");
            return false;
        }
        items.Add(item);
        RefreshInventory();
        equipmentManager.UpdateEquippedConsumables();
        
        return true;
    }

    public void Remove(CustomItem item) {
        if (item != null)
            items.Remove(item);
        
        RefreshInventory();
    }

    public void RemoveByCount(CustomItem item, int count = 1) {
        if (item != null) {
            item.itemCount -= count;

            if (item.itemCount <= 0)
                items.Remove(item);
        }
        RefreshInventory();
    }

    public void AddToInventorySlots() {
        foreach (CustomItem customItem in items) {
            GameObject slot = Instantiate(inventorySlot, itemContent);
            // GameObject item = Instantiate(inventoryItem, slot.transform);
            GameObject item = Instantiate(inventoryItem, slot.transform.Find("InventorySlot").transform);

            item.GetComponent<DraggableItem>().item = customItem;
            Text itemCountText = item.transform.Find("ItemCount").GetComponent<Text>();
            Text itemLevelText = item.transform.Find("ItemLevel").GetComponent<Text>();
            Image itemImage = item.transform.Find("ItemImage").GetComponent<Image>();
            
            if (customItem.item.isStackable && customItem.itemCount > 1)
                itemCountText.text = customItem.itemCount.ToString();
            else
                itemCountText.text = "";

            if (customItem.item.itemLevel > 0)
                itemLevelText.text = customItem.itemLevel.ToString();
            else
                itemLevelText.text = "";

            itemImage.sprite = customItem.item.itemIcon;


            NewTooltip newTooltip = slot.transform.Find("InventoryTooltip").GetComponent<NewTooltip>();
            newTooltip.SetTooltipItem(customItem, equipmentManager, true);

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

    public void SortItemsByLevelAndRarity() {
        items.Sort(new CustomItemComparer());
    }

    public void ShowInventory(bool show) {
        if (show) {
            showAllTypes = true;
            currentItemTypes = null;
            inventoryUI.SetActive(true);
            SortItemsByLevelAndRarity();
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
            SortItemsByLevelAndRarity();
            AddToInventorySlots();
            AddInventorySlots(inventorySlotCount - items.Count);
        } 
        else {
            SortItemsByLevelAndRarity();
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
                //GameObject item = Instantiate(inventoryItem, slot.transform);
                GameObject item = Instantiate(inventoryItem, slot.transform.Find("InventorySlot").transform);

                item.GetComponent<DraggableItem>().item = customItem;
                Text itemCountText = item.transform.Find("ItemCount").GetComponent<Text>();
                Text itemLevelText = item.transform.Find("ItemLevel").GetComponent<Text>();
                Image itemImage = item.transform.Find("ItemImage").GetComponent<Image>();
                
                if (customItem.item.isStackable && customItem.itemCount > 1)
                    itemCountText.text = customItem.itemCount.ToString();
                else 
                    itemCountText.text = "";

                if (customItem.item.itemLevel > 0)
                    itemLevelText.text = customItem.itemLevel.ToString();
                else
                    itemLevelText.text = "";

                itemImage.sprite = customItem.item.itemIcon;
            }
        }

        AddInventorySlots(inventorySlotCount - items.Count);
    }
}