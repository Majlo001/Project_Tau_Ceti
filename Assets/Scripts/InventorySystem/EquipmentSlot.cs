using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class EquipmentSlot : MonoBehaviour, IDropHandler {

    public string slotItemType;
    private InventoryManager inventoryManager;
    public CustomItem item;
    
    void Start() {
        inventoryManager = GameObject.Find("InventoryManager").GetComponent<InventoryManager>();
    }

    public void OnDrop(PointerEventData eventData) {
        // TODO: Swapping items

        if (transform.childCount > 0) {
            return;
        }

        GameObject droppedItem = eventData.pointerDrag;
        DraggableItem draggableItem = droppedItem.GetComponent<DraggableItem>();


        if (slotItemType != null) {
            if (draggableItem.item.item.itemType == ItemTypeDictionary.Instance.GetItemTypeByKey(slotItemType)) {
                draggableItem.parentAfterDrag = transform;
                item = draggableItem.item;
                inventoryManager.Remove(item);
            }
            else {
                Debug.Log("Wrong item type!");
            }
        } else {
            // Temporary solution
            draggableItem.parentAfterDrag = transform;
            item = draggableItem.item;
            inventoryManager.Remove(item);
        }
    }
}
