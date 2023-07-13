using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class InventoryDrop : MonoBehaviour, IDropHandler {

    private InventoryManager inventoryManager;
    public CustomItem item = null;
    
    void Start() {
        inventoryManager = GameObject.Find("InventoryManager").GetComponent<InventoryManager>();
    }

    public void OnDrop(PointerEventData eventData) {
        // if (transform.childCount > 0) {
        //     return;
        // }

        Debug.Log("InventoryDrop - OnDrop");
        GameObject droppedItem = eventData.pointerDrag;
        DraggableItem draggableItem = droppedItem.GetComponent<DraggableItem>();


        if (draggableItem.parentAfterDrag.transform.parent.name != "Inventory") {
            draggableItem.parentAfterDrag = transform;

            item = draggableItem.item;
            inventoryManager.ReturnToInvenotry(item);
            Destroy(draggableItem);
            Destroy(droppedItem);
        }
    }
}
