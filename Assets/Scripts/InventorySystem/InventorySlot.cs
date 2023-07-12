using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class InventorySlot : MonoBehaviour, IDropHandler {
    
    public void OnDrop(PointerEventData eventData) {
        Debug.Log("OnDrop");
        GameObject droppedItem = eventData.pointerDrag;
        DragDrop dragDrop = droppedItem.GetComponent<DragDrop>();
        dragDrop.parentAfterDrag = transform;
    }
}
