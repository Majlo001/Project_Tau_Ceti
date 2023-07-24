using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class EquipmentSlot : MonoBehaviour, IDropHandler {

    public string slotItemType;
    private InventoryManager inventoryManager;
    private EquipmentManager equipmentManager;
    public CustomItem item;
    
    void Start() {
        inventoryManager = GameObject.Find("InventoryManager").GetComponent<InventoryManager>();
        equipmentManager = GameObject.Find("InventoryManager").GetComponent<EquipmentManager>();
    }

    public void OnDrop(PointerEventData eventData) {
        GameObject droppedItem = eventData.pointerDrag;
        DraggableItem draggableItem = droppedItem.GetComponent<DraggableItem>();


        if (slotItemType != null && draggableItem.item.item.itemType != ItemTypeDictionary.Instance.GetItemTypeByKey(slotItemType)) {
            Debug.Log("Wrong item type!");
            return;
        }

        if (transform.childCount > 0 && item != null) {
            bool canBeReturned = inventoryManager.ReturnToInventory(item, true);

            // if (canBeReturned) {
            Transform childTransform = transform.GetChild(0);
            GameObject childObject = childTransform.gameObject;
            Destroy(childObject);
            // }
        }
        draggableItem.parentAfterDrag = transform;
        item = draggableItem.item;
        inventoryManager.Remove(item);
        equipmentManager.EquipItem(item, slotItemType);
        equipmentManager.PrintEquipment();
    }

    public void RemoveItem() {
        item = null;
    }
}
