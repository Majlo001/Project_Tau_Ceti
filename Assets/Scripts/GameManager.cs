using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour{

    public static GameManager instance;
    private PlayerController playerController;
    private InventoryManager inventoryManager;
    private EquipmentManager equipmentManager;
    private LootSystem lootSystem;

    public bool isMenuOpen = false;
    private bool isPaused = false;
    private bool isLootBoxOpen = false;
    private bool canPressEscape = true;


    private void Awake() {
        if (instance != null && instance != this) {
            Destroy(this.gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(this.gameObject);
    }

    void Start() {
        playerController = GameObject.Find("Player").GetComponent<PlayerController>();
        inventoryManager = GameObject.Find("InventoryManager").GetComponent<InventoryManager>();
        equipmentManager = GameObject.Find("InventoryManager").GetComponent<EquipmentManager>();
        lootSystem = GameObject.Find("InventoryManager").GetComponent<LootSystem>();
    }

    void Update() {

        /// Opening and closing pause menu
        if (Input.GetKeyDown(KeyCode.Escape)) {
            if (isLootBoxOpen || !canPressEscape)
                return;
            
            if (isPaused) {
                isPaused = false;

                if (!isMenuOpen)
                    ResumeGame();
            }
            else {
                isPaused = true;
                PauseGame();
            }
        }

        /// Opening and closing inventory
        if (Input.GetKeyDown(KeyCode.I)) {
            if (isPaused)
                return;

            if (isLootBoxOpen){
                hideLootBox();
            }

            if (isMenuOpen) {
                isMenuOpen = false;
                ResumeGame();
                inventoryManager.ShowInventory(false);
            }
            else {
                isMenuOpen = true;
                PauseGame();
                inventoryManager.ShowInventory(true);
            }
        }


        if (!canPressEscape) {
            isLootBoxOpen = false;
            canPressEscape = true;
        }


        /// Using Consumables 
        if (!isLootBoxOpen && !isMenuOpen && !isPaused) {
            if (Input.GetKeyDown(KeyCode.Z)) {
                equipmentManager.UseConsumable(0);
            } 
            else if (Input.GetKeyDown(KeyCode.X)) {
                equipmentManager.UseConsumable(1);
            }
            else if (Input.GetKeyDown(KeyCode.C)) {
                equipmentManager.UseConsumable(2);
            }
            else if (Input.GetKeyDown(KeyCode.V)) {
                equipmentManager.UseConsumable(3);
            }
        }
    }

    private void PauseGame() {
        Time.timeScale = 0f;
        Debug.Log("Game paused");
        playerController.SetPlayerCanMove(false);
    }

    private void ResumeGame() {
        if (!isMenuOpen)
            Time.timeScale = 1f;
        
        Debug.Log("Game resumed");
        playerController.SetPlayerCanMove(true);
    }

    public void SetLootBoxOpen(bool isOpen) {
        isLootBoxOpen = isOpen;

        if (!isOpen)
            canPressEscape = false;
    }

    public void hideLootBox() {
        isLootBoxOpen = false;
        lootSystem.ShowLootBoxUI(isLootBoxOpen);
    }
}
