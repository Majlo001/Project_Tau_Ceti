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
    public float lootRange = 2f;
    public float attackDamage = 20f;

    private bool isAttacking = false;
    private bool isCooldown = false;
    public float attackCooldown = 2f;
    private float cooldownTimer = 0f;

    private FieldOfView fieldOfView;


    private LootSystem lootSystem;
    public bool isDead = false;
    public bool isLooted = false;

    //TODO: Create and add loot system
    //Temporary solution
    public Item[] temporaryItems;
    public int[] temporaryItemsCount;
    public List<CustomItem> lootItems = new List<CustomItem>();



    void Start() {
        for (int i = 0; i < temporaryItems.Length; i++) {
            CustomItem newCustomItem = new CustomItem(temporaryItems[i], temporaryItemsCount[i]);
            lootItems.Add(newCustomItem);
        }
        temporaryItems = null;
        temporaryItemsCount = null;


        currentHealth = maxHealth;
        _healthBar.UpdateHealthBar(currentHealth, maxHealth);

        agent = GetComponent<NavMeshAgent>();
        fieldOfView = GetComponent<FieldOfView>();

        lootSystem = FindObjectOfType<LootSystem>();
    }

    void Update() {
        if (!isDead) {
            float distanceToPlayer = Vector3.Distance(transform.position, player.position);

            if (fieldOfView.canSeePlayer) {
                agent.destination = player.position;
            }

            if (distanceToPlayer <= attackRange && !isAttacking) {
                agent.isStopped = true;
                AttackPlayer();
            }
            else if (distanceToPlayer > attackRange) {
                agent.isStopped = false;
                // agent.destination = player.position;
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
        else {
            if (!isLooted) {
                float distanceToPlayer = Vector3.Distance(transform.position, player.position);

                if (distanceToPlayer <= lootRange) {
                    if (Input.GetKeyDown(KeyCode.E) && !lootSystem.isLootBoxOpen) {
                        lootSystem.InitializeLootBox(lootItems, transform.GetComponent<Enemy>());
                    }
                }
            }
        }
    }

    void AttackPlayer() {
        transform.LookAt(player);

        //TODO: Animation
        Debug.Log("Enemy attack");
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
            // Destroy(gameObject);
            isDead = true;
            _healthBar.SetHealthBarActive(false);
        }
        else {
            _healthBar.UpdateHealthBar(currentHealth, maxHealth);
        }
    }
}
