using System.Collections;
using UnityEngine;

public class EnemyBehaviour : MonoBehaviour, IDamageable
{
    [Header("Enemy Settings")]
    public float moveSpeed = 3f;          // Speed of the enemy's movement
    public float attackRange = 1.5f;     // Range within which the enemy can attack
    public float attackCooldown = 2f;    // Time between attacks
    public int damageAmount = 10;        // Damage dealt to the player
    public int health = 50;              // Enemy health

    // References and state
    private Transform player;            // Reference to the player
    private bool isAttacking = false;    // Whether the enemy is currently attacking
    private float checkInterval = 0.2f;  // Check interval for updates
    private float nextCheckTime = 0f;

    // Damage feedback
    private SpriteRenderer spriteRenderer;
    private Color originalColor;

    void Start()
    {
        // Find the player
        GameObject playerObject = GameObject.FindGameObjectWithTag("Player");
        if (playerObject != null)
        {
            player = playerObject.transform;
        }
        else
        {
            Debug.LogError("Player not found! Make sure your player has the 'Player' tag.");
        }

        // Initialize sprite renderer for damage feedback
        spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer != null)
        {
            originalColor = spriteRenderer.color;
        }
    }

    void Update()
    {
        if (Time.time >= nextCheckTime)
        {
            nextCheckTime = Time.time + checkInterval;

            if (player == null) return;

            float distanceToPlayer = Vector3.Distance(transform.position, player.position);

            if (distanceToPlayer <= attackRange && !isAttacking)
            {
                StartCoroutine(Attack());
            }
            else
            {
                MoveTowardsPlayer();
            }
        }
    }

    void MoveTowardsPlayer()
    {
        if (player == null) return;

        Vector3 direction = (player.position - transform.position).normalized;
        transform.position += direction * moveSpeed * Time.deltaTime;
    }

    IEnumerator Attack()
    {
        isAttacking = true;

        Debug.Log("Enemy attacks the player!");

        // Apply damage to the player
        ApplyDamageToPlayer();

        yield return new WaitForSeconds(attackCooldown);
        isAttacking = false;
    }

    private void ApplyDamageToPlayer()
    {
        // Find the player controller (replace PlayerController with your actual player script name)
        PlayerController playerController = Object.FindFirstObjectByType<PlayerController>();
        if (playerController != null)
        {
            playerController.TakeDamage(damageAmount);
        }
    }

    // IDamageable implementation: Take damage
    public void TakeDamage(int damage)
    {
        health -= damage;
        Debug.Log($"Enemy received damage: {damage}, Current health: {health}");

        if (health <= 0)
        {
            Die();
        }
        else
        {
            StartCoroutine(FlashRed(0.2f));
        }
    }

    private void Die()
    {
        Debug.Log("Enemy died!");
        Destroy(gameObject);
    }

    private IEnumerator FlashRed(float duration)
    {
        if (spriteRenderer != null)
        {
            spriteRenderer.color = Color.red;
            yield return new WaitForSeconds(duration);
            spriteRenderer.color = originalColor;
        }
    }
}
