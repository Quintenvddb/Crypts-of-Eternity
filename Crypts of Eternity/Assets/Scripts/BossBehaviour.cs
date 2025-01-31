using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BossBehaviour : MonoBehaviour, IDamageable
{
    private Transform player;
    public float speed = 3f;
    private Vector2 currentDirection;
    private float lastUpdateTime = 0f;
    public float minUpdateInterval = 0.3f;
    private float updateInterval;
    private int health = 200;
    private SpriteRenderer spriteRenderer;
    private Color originalColor;

    public int damageAmount = 20;
    private Rigidbody2D rb;

    public float attackCooldown = 1.5f;
    private float lastAttackTime = 0f;

    private bool isCharging = false;
    private float chargeTime = 2f;
    private float chargeSpeed = 14f;

    public AudioSource audioSource;
    public AudioClip deathAudio;
    public float deathVolume = 1.0f;
    private BossFightController bossFightController;

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

        bossFightController = Object.FindFirstObjectByType<BossFightController>();
        if (bossFightController == null)
        {
            Debug.LogWarning("BossFightController not found in the scene!");
        }

        StartCoroutine(ChargeRoutine());
    }

    void Update()
    {
        if (Time.time - lastUpdateTime >= updateInterval && !isCharging)
        {
            if (player != null)
            {
                currentDirection = (player.position - transform.position).normalized;
                lastUpdateTime = Time.time;
            }
        }

        if (!isCharging)
        {
            rb.linearVelocity = currentDirection * speed;
        }

        if (currentDirection.x < 0)
        {
            spriteRenderer.flipX = false;
        }
        else if (currentDirection.x > 0)
        {
            spriteRenderer.flipX = true;
        }
    }

    private IEnumerator ChargeRoutine()
    {
        while (true)
        {
            float waitTime = Random.Range(5f, 10f);
            yield return new WaitForSeconds(waitTime);

            isCharging = false;
            rb.linearVelocity = Vector2.zero;
            yield return new WaitForSeconds(1f);

            isCharging = true;

            currentDirection = (player.position - transform.position).normalized;
            float chargeDuration = chargeTime;

            while (chargeDuration > 0f)
            {
                rb.linearVelocity = currentDirection * chargeSpeed;
                chargeDuration -= Time.deltaTime;
                yield return null;
            }

            rb.linearVelocity = Vector2.zero;
            isCharging = false;
        }
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
        Debug.Log("Boss died!");
        audioSource.PlayOneShot(deathAudio, deathVolume);
        bossFightController.BossDefeated();
        Time.timeScale = 0;
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
