using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class InventorySlot : MonoBehaviour, IDropHandler {
    
    public void OnDrop(PointerEventData eventData) {
        if (transform.childCount > 0) {
            return;
        }
        
        Debug.Log("OnDrop - Slot");
        GameObject droppedItem = eventData.pointerDrag;
        DragDrop dragDrop = droppedItem.GetComponent<DragDrop>();
        dragDrop.parentAfterDrag = transform;
    }
}
