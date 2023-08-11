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

        if (draggableItem == null)
            return;

        if (draggableItem.parentAfterDrag.transform.parent.transform.parent.name != "Inventory") {
            item = draggableItem.item;
            bool canBeReturned = inventoryManager.ReturnToInventory(item);

            if(canBeReturned) {
                // draggableItem.parentAfterDrag = transform;
                
                EquipmentSlot equipmentSlot = draggableItem.parentAfterDrag.GetComponent<EquipmentSlot>();
                if (equipmentSlot != null) {
                    equipmentSlot.RemoveItem();
                }
                else {
                    ConsumableSlot consumableSlot = draggableItem.parentAfterDrag.GetComponent<ConsumableSlot>();
                    if (consumableSlot != null) {
                        consumableSlot.RemoveItem();
                    }
                }

                Destroy(draggableItem);
                Destroy(droppedItem);
            }
        }
    }
}
