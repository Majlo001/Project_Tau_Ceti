using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public float speed = 5;
    private Vector2 move;
    private Quaternion targetRotation;
    [SerializeField] private HealthManager _healthBar;

    private bool isPaused = false;

    public float currentHealth;
    public float maxHealth = 200f;

    void Start() {
        currentHealth = maxHealth;
        _healthBar.UpdateHealthBar(currentHealth, maxHealth);
    }

    void OnMove(InputAction.CallbackContext context){
        move = context.ReadValue<Vector2>();
    }

    public void movePlayer() {
        if (!isPaused) {
            Vector3 movement = new Vector3(move.x, 0f, move.y);

            if (movement != Vector3.zero){
                targetRotation = Quaternion.LookRotation(movement);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, 0.1f);
            }

            transform.Translate(movement * speed * Time.deltaTime, Space.World);
        }
    }

    void Update() {
        movePlayer();
    }

    public void TakeDamage(float damage) {
        currentHealth -= damage;
        
        if (currentHealth <= 0){
            //Destroy(gameObject);
            _healthBar.UpdateHealthBar(currentHealth, maxHealth);
        }
        else {
            _healthBar.UpdateHealthBar(currentHealth, maxHealth);
        }
    }


    /// Temporary method for testing
    void OnMouseDown() {
        currentHealth -= Random.Range(50f, 100f);

        if (currentHealth <= 0){
            //Destroy(gameObject);
            _healthBar.UpdateHealthBar(currentHealth, maxHealth);
        }
        else {
            _healthBar.UpdateHealthBar(currentHealth, maxHealth);
        }
    }

    public void SetPaused(bool value) {
        isPaused = value;
    }
}
