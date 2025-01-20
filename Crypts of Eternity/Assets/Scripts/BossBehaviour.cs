using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BossBehaviour : MonoBehaviour, IDamageable
{
    private Transform player;
    public float speed = 2.0f;
    private Vector2 currentDirection;
    private float lastUpdateTime = 0f;
    public float minUpdateInterval = 0.3f;
    private float updateInterval;
    public int health = 500;
    private SpriteRenderer spriteRenderer;
    private Color originalColor;

    public int damageAmount = 20;
    private Rigidbody2D rb;

    public EnemyLootTable LootTable;

    public float attackCooldown = 1.5f;
    private float lastAttackTime = 0f;

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
        Debug.Log($"Boss received damage: {damage}, Current health: {health}");

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
        TryAttack(collision);
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        TryAttack(collision);
    }

    private void TryAttack(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (Time.time - lastAttackTime >= attackCooldown)
            {
                PlayerController player = collision.GetComponent<PlayerController>();
                if (player != null)
                {
                    player.TakeDamage(damageAmount);

                    Vector2 knockbackDirection = (collision.transform.position - transform.position).normalized;
                    float knockbackForce = 5f;
                    player.ApplyKnockback(knockbackDirection, knockbackForce);
                }

                lastAttackTime = Time.time;
            }
        }
    }
}
