using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class DraggableItem : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler {

    public Image image;
    [HideInInspector] public Transform parentAfterDrag;
    public CustomItem item;


    public void OnBeginDrag(PointerEventData eventData) {
        Debug.Log("OnBeginDrag");
        parentAfterDrag = transform.parent;
        transform.SetParent(transform.root);
        transform.SetAsLastSibling();
        image.raycastTarget = false;
        // item = GetComponent<CustomItem>();
        Debug.Log("Item: " + item);
    }

    public void OnEndDrag(PointerEventData eventData) {
        Debug.Log("OnEndDrag");
        transform.SetParent(parentAfterDrag);
        image.raycastTarget = true;
    }

    public void OnDrag(PointerEventData eventData) {
        Debug.Log("OnDrag");
        transform.position = Input.mousePosition;
    }
}