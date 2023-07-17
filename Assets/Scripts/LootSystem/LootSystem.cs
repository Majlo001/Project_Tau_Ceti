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


    public bool isLootBoxOpen = false;
    private bool isAutoLootActive = false;

    void Start() {
        ShowLootBoxUI(false);
        TakeAllBtn.onClick.AddListener(TakeAllBtnClicked);
        TakeBtn.onClick.AddListener(TakeBtnClicked);
        CloseBtn.onClick.AddListener(CloseBtnClicked);
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
        lootBoxUI.SetActive(show);
    }

    public void InitializeLootBox(List<CustomItem> items) {
        lootItems = items;
        isLootBoxOpen = true;
        ShowLootBoxUI(true);
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


            // if (i == selectedItemIndex) {
            //     itemUI.SetOutlineActive(true);
            // }
        }
    }

     private void ClearItemGrid() {
        foreach (Transform child in itemGrid) {
            Destroy(child.gameObject);
        }
    }
}
