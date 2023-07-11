using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private bool isPaused = false;
    private PlayerController playerController;
    private InventoryManager inventoryManager;

    void Start() {
        playerController = GameObject.Find("Player").GetComponent<PlayerController>();
        inventoryManager = GameObject.Find("InventoryManager").GetComponent<InventoryManager>();
    }

    void Update() {
        if (Input.GetKeyDown(KeyCode.Escape)) {
            if (isPaused) {
                ResumeGame();
            }
            else {
                PauseGame();
            }
        }

        if (Input.GetKeyDown(KeyCode.I)) {
            if (isPaused) {
                ResumeGame();
                inventoryManager.ShowInventory(false);
            }
            else {
                PauseGame();
                inventoryManager.ShowInventory(true);
            }
        }
    }

    private void PauseGame() {
        Time.timeScale = 0f;

        // TODO: Zatrzymanie aktywności w grze
        isPaused = true;

        Debug.Log("Game paused");
        playerController.SetPaused(true);
    }

    private void ResumeGame() {
        Time.timeScale = 1f;

        // TODO: Wznowienie aktywności w grze
        isPaused = false;

        Debug.Log("Game resumed");
        playerController.SetPaused(false);
    }
}
