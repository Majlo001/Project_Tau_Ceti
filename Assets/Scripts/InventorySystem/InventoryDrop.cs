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

        Debug.Log("InventoryDrop - OnDrop");
        GameObject droppedItem = eventData.pointerDrag;
        DraggableItem draggableItem = droppedItem.GetComponent<DraggableItem>();


        if (draggableItem.parentAfterDrag.transform.parent.name != "Inventory") {
            EquipmentSlot equipmentSlot = draggableItem.parentAfterDrag.GetComponent<EquipmentSlot>();
            equipmentSlot.RemoveItem();

            draggableItem.parentAfterDrag = transform;
            item = draggableItem.item;
            inventoryManager.ReturnToInvenotry(item);
            Destroy(draggableItem);
            Destroy(droppedItem);
        }
    }
}
