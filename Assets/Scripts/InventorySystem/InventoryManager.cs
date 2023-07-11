using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryManager : MonoBehaviour {

    public static InventoryManager instance;
    public List<Item> items = new List<Item>();
    // public Dictionary<Item, int> items = new Dictionary<Item, int>();


    public Transform itemContent;
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

    public void Add(Item item) {
        items.Add(item);
    }

    public void Remove(Item item) {
        items.Remove(item);
    }

    public void ListItems() {
        foreach (Item item in items) {
            GameObject itemObject = Instantiate(inventoryItem, itemContent);
            // var itemCount = itemObject.transform.Find("Count").GetComponent<Text>();
            var itemIcon = itemObject.transform.Find("ItemImage").GetComponent<Image>();

            // itemCount.text = item.itemCount;
            itemIcon.sprite = item.itemIcon;

            // Image itemIcon = itemObject.GetComponentInChildren<Image>();
            // itemIcon.sprite = item.itemIcon;
        }
    }

    public void ShowInventory(bool show) {
        if (show) {
            inventoryUI.SetActive(true);
            ListItems();
        }
        else {
            inventoryUI.SetActive(false);
        }
    }
}
