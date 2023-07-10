using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private bool isPaused = false;
    private PlayerController playerController;

    void Start(){
        playerController = GameObject.Find("Player").GetComponent<PlayerController>();
    }

    void Update(){
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused) {
                ResumeGame();
            }
            else {
                PauseGame();
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
