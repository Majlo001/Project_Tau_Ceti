using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class EquipmentSlot : MonoBehaviour, IDropHandler {

    public string slotItemType;
    private InventoryManager inventoryManager;
    private EquipmentManager equipmentManager;
    public CustomItem item;
    private NewTooltip newTooltip;
    
    void Start() {
        inventoryManager = GameObject.Find("InventoryManager").GetComponent<InventoryManager>();
        equipmentManager = GameObject.Find("InventoryManager").GetComponent<EquipmentManager>();
        newTooltip = transform.parent.transform.Find("EquipmentTooltip").GetComponent<NewTooltip>();
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

            Transform childTransform = transform.GetChild(0);
            GameObject childObject = childTransform.gameObject;
            Destroy(childObject);
        }
        draggableItem.parentAfterDrag = transform;
        item = draggableItem.item;
        inventoryManager.Remove(item);
        equipmentManager.EquipItem(item, slotItemType);
        // equipmentManager.PrintEquipment();

        newTooltip.SetTooltipItem(draggableItem.item, equipmentManager, false, true);
    }

    public void RemoveItem() {
        item = null;
        equipmentManager.UnequipItem(slotItemType);
        // Debug.Log("Item removed from slot: " + slotItemType);
    }
}
