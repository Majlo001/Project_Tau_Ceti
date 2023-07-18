using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LootSystem : MonoBehaviour {
    
    public GameObject lootBoxUI;
    public List<CustomItem> lootItems;
    public GameObject lootItemPrefab;
    public Transform itemGrid;
    public int selectedItemIndex = 0;

    public Button TakeAllBtn;
    public Button TakeBtn;
    public Button CloseBtn;


    private PlayerController playerController;
    private InventoryManager inventoryManager;

    private Enemy currentEnemy;

    public bool isLootBoxOpen = false;
    private bool isAutoLootActive = false;
    private bool canTakeItem = false;

    void Start() {
        playerController = GameObject.Find("Player").GetComponent<PlayerController>();
        inventoryManager = GameObject.Find("InventoryManager").GetComponent<InventoryManager>();

        ShowLootBoxUI(false);
        TakeAllBtn.onClick.AddListener(TakeAllBtnClicked);
        TakeBtn.onClick.AddListener(TakeBtnClicked);
        CloseBtn.onClick.AddListener(CloseBtnClicked);
    }

    void Update() {
        if (isLootBoxOpen) {
            if (Input.GetKeyDown(KeyCode.Escape)) {
                isLootBoxOpen = false;
                ShowLootBoxUI(isLootBoxOpen);
            }

            if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow)) {
                if (selectedItemIndex > 0) {
                    selectedItemIndex--;
                    DisplayLootItems();
                }
            }
            else if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow)) {
                if (selectedItemIndex < lootItems.Count - 1) {
                    selectedItemIndex++;
                    DisplayLootItems();
                }
            }
            else if (Input.GetKeyDown(KeyCode.E) && canTakeItem) {
                TakeItem(selectedItemIndex);
            }
            else if (Input.GetKeyDown(KeyCode.Space) && canTakeItem) {
                TakeAllItems();
            }
            canTakeItem = true;
        }
    }

    private void TakeAllBtnClicked() {
        ShowLootBoxUI(false);
    }
    private void TakeBtnClicked() {
        ShowLootBoxUI(false);
    }
    private void CloseBtnClicked() {
        ShowLootBoxUI(false);
    }

    public void ShowLootBoxUI(bool show) {
        selectedItemIndex = 0;
        lootBoxUI.SetActive(show);
        GameManager.instance.SetLootBoxOpen(show);
        playerController.SetPlayerCanMove(!show);
    }

    public void InitializeLootBox(List<CustomItem> items, Enemy enemy) {
        lootItems = items;
        currentEnemy = enemy;
        isLootBoxOpen = true;
        ShowLootBoxUI(isLootBoxOpen);
        DisplayLootItems();
    }

    public void DisplayLootItems() {
        ClearItemGrid();

        for (int i = 0; i < lootItems.Count; i++) {
            Debug.Log(i + ": " + lootItems[i].item.itemName);
            GameObject itemObject = Instantiate(lootItemPrefab, itemGrid);

            Text itemNameText = itemObject.transform.Find("ItemInfo/ItemName").GetComponent<Text>();
            Text itemRarityText = itemObject.transform.Find("ItemInfo/ItemRarity").GetComponent<Text>();
            Text itemCountText = itemObject.transform.Find("Item/ItemCount").GetComponent<Text>();
            Text itemLevelText = itemObject.transform.Find("Item/ItemLevel").GetComponent<Text>();
            Image itemImage = itemObject.transform.Find("Item/ItemImage").GetComponent<Image>();

            itemNameText.text = lootItems[i].item.itemName;
            itemRarityText.text = ItemRarityDictionary.Instance.itemRarity[lootItems[i].item.itemRarity];
            itemRarityText.color = ItemRarityDictionary.Instance.HexToColor(ItemRarityDictionary.Instance.itemColors[lootItems[i].item.itemRarity]);
            itemLevelText.text = lootItems[i].item.itemLevel.ToString();
            itemImage.sprite = lootItems[i].item.itemIcon;


            if (i == selectedItemIndex) {
                Outline outline = itemObject.GetComponent<Outline>();
                if (outline != null)
                    outline.enabled = true;
            }
        }
    }

     private void ClearItemGrid() {
        foreach (Transform child in itemGrid) {
            Destroy(child.gameObject);
        }
    }


    private void TakeItem(int index) {
        if (lootItems.Count > 0) {
            CustomItem item = lootItems[index];
            inventoryManager.AddItem(item);
            lootItems.RemoveAt(index);
            currentEnemy.lootItems.RemoveAt(index);
            DisplayLootItems();

            if (lootItems.Count == 0) {
                isLootBoxOpen = false;
                ShowLootBoxUI(isLootBoxOpen);
                currentEnemy.isLooted = true;
            }
        }
    }

    private void TakeAllItems() {
        if (lootItems.Count > 0) {
            foreach (CustomItem item in lootItems) {
                inventoryManager.AddItem(item);
            }
            lootItems.Clear();
            currentEnemy.lootItems.Clear();
            currentEnemy.isLooted = true;
            isLootBoxOpen = false;
            ShowLootBoxUI(isLootBoxOpen);
        }
    }

    private IEnumerator DelayedAction(float time) {
        yield return new WaitForSeconds(time);
    }
}
