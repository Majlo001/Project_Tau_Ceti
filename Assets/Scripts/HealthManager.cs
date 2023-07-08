using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthManager : MonoBehaviour {
    [SerializeField] private Image _healthBar;

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    //public void takedamage(float damage, float maxhealth){
    //    healthamount -= damage;
    //    _healthbar.fillamount = healthamount/maxhealth;
    //}
    public void UpdateHealthBar(float currentHealth, float maxHealth) {
        _healthBar.fillAmount = currentHealth / maxHealth;
    }

    //public void Heal(float amount, float maxHealth){
    //    healthAmount += amount;
    //    healthAmount = Mathf.Clamp(healthAmount, 0, maxHealth);
    //    
    //    _healthBar.fillAmount = healthAmount/maxHealth;
    //}
}
