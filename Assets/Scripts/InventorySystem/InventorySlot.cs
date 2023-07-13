using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class InventorySlot : MonoBehaviour, IDropHandler {

    // public Type? allowedItemType;
    
    public void OnDrop(PointerEventData eventData) {
        if (transform.childCount > 0) {
            return;
        }

        Debug.Log("OnDrop - Slot");
        GameObject droppedItem = eventData.pointerDrag;
        DraggableItem draggableItem = droppedItem.GetComponent<DraggableItem>();
        draggableItem.parentAfterDrag = transform;
    }
}
