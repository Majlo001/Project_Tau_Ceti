using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using System.Linq;

public class ConsumableSlot : MonoBehaviour, IDropHandler {

    public int slotNumber;
    private InventoryManager inventoryManager;
    private EquipmentManager equipmentManager;
    public CustomItem item;
    private NewTooltip newTooltip;
    
    void Start() {
        inventoryManager = GameObject.Find("InventoryManager").GetComponent<InventoryManager>();
        equipmentManager = GameObject.Find("InventoryManager").GetComponent<EquipmentManager>();
        newTooltip = transform.parent.transform.Find("ConsumableTooltip").GetComponent<NewTooltip>();
    }

    //TODO: Double click to use item.

    public void OnDrop(PointerEventData eventData) {
        GameObject droppedItem = eventData.pointerDrag;
        DraggableItem draggableItem = droppedItem.GetComponent<DraggableItem>();


        
        if (!ItemTypeDictionary.Instance.itemTypeConsumables.Contains(draggableItem.item.item.itemType)) {
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
            parentObject.transform.GetComponent<ConsumableSlot>().ChangeTooltip(childObject.GetComponent<DraggableItem>());
        }
        else if (transform.childCount > 0 && item != null) {
            bool canBeReturned = inventoryManager.ReturnToInventory(item, true);

            GameObject childObject = transform.GetChild(0).gameObject;
            Destroy(childObject);
        }

        draggableItem.parentAfterDrag = transform;
        item = draggableItem.item;
        inventoryManager.Remove(item);
        equipmentManager.EquipConsumable(item, slotNumber);
        ChangeTooltip(draggableItem);
    }

    public void ChangeTooltip(DraggableItem draggableItem) {
        newTooltip.SetTooltipItem(draggableItem.item, equipmentManager, false, true);
    }

    public void RemoveItem() {
        item = null;
        equipmentManager.UnequipConsumable(slotNumber);
    }

    public void RemoveDraggableItem() {
        if (transform.childCount > 0) {
            GameObject childObject = transform.GetChild(0).gameObject;
            Destroy(childObject);
        }
    }
}
