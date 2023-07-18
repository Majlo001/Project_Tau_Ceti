using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour{

    public static GameManager instance;
    private PlayerController playerController;
    private InventoryManager inventoryManager;

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
    }

    void Update() {
        if (Input.GetKeyDown(KeyCode.Escape)) {
            if (isLootBoxOpen || !canPressEscape)
                return;
            
            if (isPaused) {
                isPaused = false;
                ResumeGame();
            }
            else {
                isPaused = true;
                PauseGame();
            }
        }

        if (Input.GetKeyDown(KeyCode.I)) {
            if (isPaused)
                return;

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

        if (!canPressEscape)
            isLootBoxOpen = false;
            canPressEscape = true;
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


}
