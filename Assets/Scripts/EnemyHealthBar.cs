using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class EnemyHealthBar : MonoBehaviour {
    [SerializeField] private Image _healthbarSprite;

    private Camera _cam;

    void Start(){
        _cam = Camera.main;
    }

    public void UpdateHealthBar(float currentHealth, float maxHealth) {
        _healthbarSprite.fillAmount = currentHealth / maxHealth;
    }

    public void SetHealthBarActive(bool active) {
        _healthbarSprite.gameObject.SetActive(active);
    }

    void Update() {
        transform.rotation = Quaternion.LookRotation(transform.position - _cam.transform.position);
    }
}
