using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryButtons : MonoBehaviour {
    
    public Button AllBtn;
    public Button WeaponsBtn;
    public Button ShieldsBtn;
    public Button ArmourBtn;
    public Button ConsumablesBtn;
    public Button MaterialsBtn;
    public Button QuestItemsBtn;

    private InventoryManager inventoryManager;

    void Start() {
        AllBtn.onClick.AddListener(() => inventoryManager.showAllItemTypes());
        WeaponsBtn.onClick.AddListener(WeaponsBtnClick);
        ShieldsBtn.onClick.AddListener(ShieldsBtnClick);
        ArmourBtn.onClick.AddListener(ArmourBtnClick);
        ConsumablesBtn.onClick.AddListener(ConsumablesBtnClick);
        MaterialsBtn.onClick.AddListener(MaterialsBtnClick);
        QuestItemsBtn.onClick.AddListener(QuestItemsBtnClick);

        inventoryManager = GameObject.Find("InventoryManager").GetComponent<InventoryManager>();
    }

    private void WeaponsBtnClick() {
        int[] itemTypes = ItemTypeDictionary.Instance.itemTypeWeapons;
        inventoryManager.ShowItemsByType(itemTypes);
    }

    private void ShieldsBtnClick() {
        int[] itemTypes = ItemTypeDictionary.Instance.itemTypeShields;
        inventoryManager.ShowItemsByType(itemTypes);
    }

    private void ArmourBtnClick() {
        int[] itemTypes = ItemTypeDictionary.Instance.itemTypeArmour;
        inventoryManager.ShowItemsByType(itemTypes);
    }

    private void ConsumablesBtnClick() {
        int[] itemTypes = ItemTypeDictionary.Instance.itemTypeConsumables;
        inventoryManager.ShowItemsByType(itemTypes);
    }

    private void MaterialsBtnClick() {
        int[] itemTypes = ItemTypeDictionary.Instance.itemTypeMaterials;
        inventoryManager.ShowItemsByType(itemTypes);
    }

    private void QuestItemsBtnClick() {
        int[] itemTypes = ItemTypeDictionary.Instance.itemTypeQuestItems;
        inventoryManager.ShowItemsByType(itemTypes);
    }
}
