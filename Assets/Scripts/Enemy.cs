using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour {
    public float maxHealth = 200f;
    private float currentHealth;

    public Transform player;
    private NavMeshAgent agent;

    [SerializeField] private EnemyHealthBar _healthBar;

    public float attackRange = 2f;
    public float attackDamage = 20f;

    private bool isAttacking = false;
    private bool isCooldown = false;
    public float attackCooldown = 2f;
    private float cooldownTimer = 0f;



    void Start()
    {
        currentHealth = maxHealth;
        _healthBar.UpdateHealthBar(currentHealth, maxHealth);

        agent = GetComponent<NavMeshAgent>();
    }

    void Update()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        if (distanceToPlayer <= attackRange && !isAttacking) {
            agent.isStopped = true;
            AttackPlayer();
        }
        else if (distanceToPlayer > attackRange) {
            agent.isStopped = false;
            agent.destination = player.position;
        }

        if (isCooldown) {
            cooldownTimer += Time.deltaTime;

            if (cooldownTimer >= attackCooldown)
            {
                isCooldown = false;
                cooldownTimer = 0f;
            }
        }
    }

    void AttackPlayer()
    {
        transform.LookAt(player);

        // Wykonaj atak (mo¿esz dodaæ tu odpowiedni¹ animacjê ataku)
        Debug.Log("Przeciwnik atakuje gracza!");
        player.GetComponent<PlayerController>().TakeDamage(attackDamage);

        
        isAttacking = true;
        StartCoroutine(AttackCooldown());
    }

    IEnumerator AttackCooldown() {
        isCooldown = true;
        float chaseTimer = 0f;

        while (chaseTimer < attackCooldown)
        {
            chaseTimer += Time.deltaTime;
            agent.destination = player.position;
            yield return null;
        }

        isCooldown = false;
        isAttacking = false;
    }

    void OnMouseDown() {
        currentHealth -= Random.Range(50f, 100f);

        if (currentHealth <= 0){
            Destroy(gameObject);
        }
        else {
            _healthBar.UpdateHealthBar(currentHealth, maxHealth);
        }
    }
}
