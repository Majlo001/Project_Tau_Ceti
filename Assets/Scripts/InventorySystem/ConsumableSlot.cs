using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ConsumableSlot : MonoBehaviour, IDropHandler {

    // private string slotItemType;
    public int slotNumber;
    private InventoryManager inventoryManager;
    private EquipmentManager equipmentManager;
    public CustomItem item;
    
    void Start() {
        inventoryManager = GameObject.Find("InventoryManager").GetComponent<InventoryManager>();
        equipmentManager = GameObject.Find("InventoryManager").GetComponent<EquipmentManager>();
        // slotItemType = "Consumable";
    }

    //TODO: Double click to use item.

    public void OnDrop(PointerEventData eventData) {
        GameObject droppedItem = eventData.pointerDrag;
        DraggableItem draggableItem = droppedItem.GetComponent<DraggableItem>();


        if (!draggableItem.item.item.isConsumable) {
            Debug.Log("Wrong item type!");
            return;
        }

        


        GameObject parentObject = draggableItem.parentAfterDrag.gameObject;
        ConsumableSlot consumableParentSlot = parentObject.GetComponent<ConsumableSlot>();

        /// Swapping items
        if (consumableParentSlot != null) {
            equipmentManager.SwapConsumable(consumableParentSlot.slotNumber, slotNumber);
            consumableParentSlot.item = item;
            
            if (item == null) {
                consumableParentSlot.RemoveItem();
            }

            draggableItem.parentAfterDrag = transform;
            item = draggableItem.item;
            draggableItem.transform.SetParent(transform);

            GameObject childObject = transform.GetChild(0).gameObject;
            childObject.transform.SetParent(parentObject.transform);
        }
        else if (transform.childCount > 0 && item != null) {
            inventoryManager.ReturnToInvenotry(item);

            GameObject childObject = transform.GetChild(0).gameObject;
            Destroy(childObject);
        }

        draggableItem.parentAfterDrag = transform;
        item = draggableItem.item;
        inventoryManager.Remove(item);
        equipmentManager.EquipConsumable(item, slotNumber);
    }

    public void RemoveItem() {
        item = null;
        equipmentManager.UnequipConsumable(slotNumber);
    }
}