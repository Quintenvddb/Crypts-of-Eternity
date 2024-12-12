using System.Collections;
using UnityEngine;

public class EnemyBehaviour : MonoBehaviour
{
<<<<<<< Updated upstream
    public float moveSpeed = 2f;
    private Vector2 moveDirection;
    private float directionChangeInterval = 0.2f; // Time interval to change direction
    private float timer = 0f;

    void Start()
    {
        ChooseRandomDirection(); // Initialize with a random direction
=======
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
>>>>>>> Stashed changes
    }

    void Update()
    {
<<<<<<< Updated upstream
        // Update the timer
        timer += Time.deltaTime;

        // If it's time to change direction
        if (timer >= directionChangeInterval)
        {
            ChooseRandomDirection(); // Change direction
            timer = 0f; // Reset timer
        }

        // Move the enemy in the chosen direction
        transform.Translate(moveDirection * moveSpeed * Time.deltaTime);
    }

    // Choose a random direction (left, right, up, down)
    void ChooseRandomDirection()
    {
        float randomValue = Random.Range(0f, 4f); // Random number between 0 and 4

        if (randomValue < 1f)
            moveDirection = Vector2.left; // Move left
        else if (randomValue < 2f)
            moveDirection = Vector2.right; // Move right
        else if (randomValue < 3f)
            moveDirection = Vector2.up; // Move up
        else
            moveDirection = Vector2.down; // Move down
=======
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

        Quaternion lookRotation = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f);
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
>>>>>>> Stashed changes
    }
}
