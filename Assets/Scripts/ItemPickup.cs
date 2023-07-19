using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemPickup : MonoBehaviour
{
    public float pickupDistance = 3f;
    public float pickupDuration = 3f;
    public float decreaseSpeed = 1f;

    // private bool canBePickedUp = false;
    // private bool isBeingPickedUp = false;
    private float pickupTimer = 0f;

    public Transform player;
    public GameObject canvasObject;
    public Slider slider;


    public Item item;


    void Start() {
        canvasObject.SetActive(false);
        slider.value = 0f;
    }

    
    private void Update() {
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        if (distanceToPlayer <= pickupDistance) {
            if (Input.GetKey(KeyCode.E)) {
                pickupTimer += Time.deltaTime;
                slider.value = pickupTimer / pickupDuration;

                if (pickupTimer >= pickupDuration) {
                    Collect();
                }
            }
            else {
                if (pickupTimer > 0f)
                {
                    pickupTimer -= Time.deltaTime * decreaseSpeed;
                    slider.value = pickupTimer / pickupDuration;
                }
            }

            ShowTooltip();
        }
        else {
            HideTooltip();
            ResetPickup();
        }
    }

    private void ShowTooltip() {
        canvasObject.SetActive(true);
        // canBePickedUp = true;
    }

    private void HideTooltip() {
        canvasObject.SetActive(false);
        // canBePickedUp = false;
        ResetPickup();
    }

    private void ResetPickup() {
        pickupTimer = 0f;
        slider.value = 0f;
    }

    private void Collect() {
        canvasObject.SetActive(false);
        bool canBeCollected = InventoryManager.instance.AddItem(item);

        if (canBeCollected)
            Destroy(gameObject);
        else
            Debug.Log("Inventory is full");
    }
}
