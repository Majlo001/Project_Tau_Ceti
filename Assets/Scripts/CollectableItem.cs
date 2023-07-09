using System.Collections;
using System.Collections.Generic;
using UnityEngine;

interface IInteractable {
    public void Interact();
}

/**
    * This class is used to interact with objects in the game world.
    * It uses a raycast to detect objects in front of the player.
    * If the object is interactable, it will call the Interact() method
    * on the object.
**/


public class CollectableItem : MonoBehaviour
{
    public Transform InteractorSource;
    public float InteractRange;


    void Start() {
        
    }


    void Update(){
        if (Input.GetKeyDown(KeyCode.E)){
            Ray r = new Ray(InteractorSource.position, InteractorSource.forward);

            if (Physics.Raycast(r, out RaycastHit hitInfo, InteractRange)) {
                if (hitInfo.collider.gameObject.TryGetComponent(out IInteractable interactObj)) {
                    interactObj.Interact();
                }
            }
        }
    }
}
