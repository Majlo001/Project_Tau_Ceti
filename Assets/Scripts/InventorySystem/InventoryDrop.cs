using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class InventoryDrop : MonoBehaviour, IDropHandler {

    private InventoryManager inventoryManager;
    public CustomItem item;
    
    void Start() {
        inventoryManager = GameObject.Find("InventoryManager").GetComponent<InventoryManager>();
    }

    public void OnDrop(PointerEventData eventData) {
        GameObject droppedItem = eventData.pointerDrag;
        DraggableItem draggableItem = droppedItem.GetComponent<DraggableItem>();

        if (draggableItem == null) {
            return;
        }

        if (draggableItem.parentAfterDrag.transform.parent.name != "Inventory") {
            EquipmentSlot slot = draggableItem.parentAfterDrag.GetComponent<EquipmentSlot>();
            if (slot != null) {
                slot.RemoveItem();
            }
            else {
                ConsumableSlot consumableSlot = draggableItem.parentAfterDrag.GetComponent<ConsumableSlot>();
                if (consumableSlot != null) {
                    consumableSlot.RemoveItem();
                }
            }

            draggableItem.parentAfterDrag = transform;
            item = draggableItem.item;
            inventoryManager.ReturnToInvenotry(item);
            Destroy(draggableItem);
            Destroy(droppedItem);
        }
    }
}
