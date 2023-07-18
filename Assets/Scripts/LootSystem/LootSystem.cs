using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LootSystem : MonoBehaviour {
    

    private ScrollRect scrollRect;
    // private RectTransform contentPanel;
    public float scrollAmountPixels = 120f;

    public GameObject lootBoxUI;
    public GameObject lootBoxScrollArea;
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
        scrollRect = lootBoxScrollArea.GetComponent<ScrollRect>();
        // contentPanel = scrollRect.content;

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
                    ScrollByPixels(scrollAmountPixels);
                }
            }
            else if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow)) {
                if (selectedItemIndex < lootItems.Count - 1) {
                    selectedItemIndex++;
                    DisplayLootItems();
                    ScrollByPixels(-scrollAmountPixels);
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
        else 
            canTakeItem = false;
    }

    private void TakeAllBtnClicked() {
        TakeAllItems();
    }
    private void TakeBtnClicked() {
        TakeItem(selectedItemIndex);
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
            
            if (lootItems[i].itemCount > 0)
                itemCountText.text = lootItems[i].itemCount.ToString();
            else
                itemCountText.text = "";

            if (lootItems[i].itemLevel > 1)
                itemLevelText.text = lootItems[i].itemLevel.ToString();
            else
                itemLevelText.text = "";

            itemImage.sprite = lootItems[i].item.itemIcon;


            if (i == selectedItemIndex) {
                Outline outline = itemObject.GetComponent<Outline>();
                if (outline != null)
                    outline.enabled = true;
            }

            // ScrollToSelectedItem(selectedItemIndex);
            // contentPanel.anchoredPosition = Vector2.zero;
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
            currentEnemy.lootItems.RemoveAt(index);

            if (lootItems.Count == 0) {
                isLootBoxOpen = false;
                ShowLootBoxUI(isLootBoxOpen);
                currentEnemy.isLooted = true;
            }

            if (index == lootItems.Count)
                selectedItemIndex--;
            
            DisplayLootItems();
        }
    }

    private void TakeAllItems() {
        if (lootItems.Count > 0) {
            foreach (CustomItem item in lootItems) {
                inventoryManager.AddItem(item);
            }
            currentEnemy.lootItems.Clear();
            currentEnemy.isLooted = true;
            isLootBoxOpen = false;
            ShowLootBoxUI(isLootBoxOpen);
        }
    }

    private void ScrollByPixels(float pixels) {

        // Temporary solution
        float normalizedScrollAmount = pixels / (scrollRect.content.rect.height - 120f);
        // float normalizedScrollAmount = pixels / (scrollRect.content.rect.height - lootItemPrefab.GetComponent<Renderer>().bounds.size.y);
        Debug.Log(normalizedScrollAmount + " " + scrollRect.content.rect.height);
        scrollRect.verticalNormalizedPosition += normalizedScrollAmount;
        scrollRect.verticalNormalizedPosition = Mathf.Clamp01(scrollRect.verticalNormalizedPosition);
    }



    // public void SnapTo(RectTransform target) {
    //     Canvas.ForceUpdateCanvases();

    //     contentPanel.anchoredPosition = (Vector2)scrollRect.transform.InverseTransformPoint(contentPanel.position) - (Vector2)scrollRect.transform.InverseTransformPoint(target.position);
    // }

    // public void ScrollToSelectedItem(int selectedItemIndex) {
    //     if (contentPanel.childCount <= selectedItemIndex)
    //         return;

    //     RectTransform selectedItem = contentPanel.GetChild(selectedItemIndex).GetComponent<RectTransform>();
    //     SnapTo(selectedItem);
    // }
}
