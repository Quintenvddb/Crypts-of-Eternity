using System.Collections;
using UnityEngine;

public class EnemyBehaviour : MonoBehaviour
{
    public float moveSpeed = 3f;          // Speed of the enemy's movement
    public float attackRange = 1.5f;     // Range within which the enemy can attack
    public float attackCooldown = 2f;    // Time between attacks
    public int damageAmount = 10;        // Damage dealt to the player

    private Transform player;            // Reference to the player
    private bool isAttacking = false;    // Whether the enemy is currently attacking

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;

        if (player == null)
        {
            Debug.LogError("Player not found! Make sure your player has the 'Player' tag.");
        }
    }

    void Update()
    {
        if (player != null)
        {
            float distanceToPlayer = Vector3.Distance(transform.position, player.position);

            if (distanceToPlayer <= attackRange)
            {
                if (!isAttacking)
                {
                    StartCoroutine(Attack());
                }
            }
            else
            {
                MoveTowardsPlayer();
            }
        }
    }

    void MoveTowardsPlayer()
    {
        Vector3 direction = (player.position - transform.position).normalized;
        transform.position += direction * moveSpeed * Time.deltaTime;
    }

    IEnumerator Attack()
    {
        isAttacking = true;

        Debug.Log("Enemy attacks the player!");

        // Apply damage using the DamageTest logic
        ApplyDamageToPlayer();

        yield return new WaitForSeconds(attackCooldown);
        isAttacking = false;
    }

    private void ApplyDamageToPlayer()
    {
        // Find the PlayerController (replace PlayerController with your actual player script name)
        PlayerController player = Object.FindFirstObjectByType<PlayerController>();
        if (player != null)
        {
            player.TakeDamage(damageAmount);
        }
    }
}
