using UnityEngine;

public class EnemyBehavior : MonoBehaviour
{
    public float moveSpeed = 2f;
    private Vector2 moveDirection;
    private float directionChangeInterval = 0.2f; // Time interval to change direction
    private float timer = 0f;

    void Start()
    {
        ChooseRandomDirection(); // Initialize with a random direction
    }

    void Update()
    {
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
    }
}
