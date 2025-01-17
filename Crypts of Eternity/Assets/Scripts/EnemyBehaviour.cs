using UnityEngine;
using System.Collections;
using System.Collections.Generic; // Vergeet niet om de lijstbibliotheek te importeren

public class EnemyBehaviour : MonoBehaviour, IDamageable
{
    private Transform player;
    public float speed = 1.5f;
    private Vector2 currentDirection;
    private float lastUpdateTime = 0f;
    public float minUpdateInterval = 0.4f;
    private float updateInterval;
    public int health = 50;
    private SpriteRenderer spriteRenderer;
    private Color originalColor;

    public int damageAmount = 10;
    private Rigidbody2D rb;

    public EnemyLootTable LootTable;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        if (rb == null)
        {
            rb = gameObject.AddComponent<Rigidbody2D>();
        }

        rb.bodyType = RigidbodyType2D.Dynamic;
        rb.gravityScale = 0;
        rb.freezeRotation = true;

        GameObject playerObject = GameObject.FindWithTag("Player");
        if (playerObject != null)
        {
            player = playerObject.transform;
        }

        updateInterval = Random.Range(0f, 0.5f) + minUpdateInterval;

        spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer != null)
        {
            originalColor = spriteRenderer.color;
        }
    }

    void Update()
    {
        if (Time.time - lastUpdateTime >= updateInterval)
        {
            if (player != null)
            {
                currentDirection = (player.position - transform.position).normalized;
                lastUpdateTime = Time.time;
            }
        }

        rb.linearVelocity = currentDirection * speed;
    }

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

        if (LootTable != null)
        {
            GameObject loot = LootTable.GetRandomLoot();
            if (loot != null)
            {
                Instantiate(loot, transform.position, Quaternion.identity);
            }
        }

        Destroy(gameObject);
    }


    private IEnumerator FlashRed(float duration)
    {
        if (spriteRenderer != null)
        {
            Color flashColor = Color.red;
            spriteRenderer.color = flashColor;
            yield return new WaitForSeconds(duration);
            spriteRenderer.color = originalColor;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            PlayerController player = Object.FindFirstObjectByType<PlayerController>();
            if (player != null)
            {
                player.TakeDamage(damageAmount);
            }
        }
    }
}
