using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class EquipmentSlot : MonoBehaviour, IDropHandler {

    // public Type? allowedItemType;
    private InventoryManager inventoryManager;
    public CustomItem item = null;
    
    void Start() {
        inventoryManager = GameObject.Find("InventoryManager").GetComponent<InventoryManager>();
    }

    public void OnDrop(PointerEventData eventData) {
        if (transform.childCount > 0) {
            return;
        }

        Debug.Log("OnDrop - Slot");
        GameObject droppedItem = eventData.pointerDrag;
        DraggableItem draggableItem = droppedItem.GetComponent<DraggableItem>();
        draggableItem.parentAfterDrag = transform;

        item = draggableItem.item;
        inventoryManager.Remove(item);
    }
}
