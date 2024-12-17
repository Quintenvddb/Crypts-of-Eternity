using UnityEngine;
using System.Collections;

public class EnemyDamageTest : MonoBehaviour, IDamageable
{
    public int health = 50;
    private SpriteRenderer spriteRenderer;
    private Color originalColor;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer != null)
        {
            originalColor = spriteRenderer.color;
        }
    }

    public void TakeDamage(int damage)
    {
        health -= damage;
        Debug.Log($"EnemyDamageTest received damage: {damage}, Current health: {health}");

        if (health <= 0)
        {
            Die();
        }
        else
        {
            StartCoroutine(FlashWhite(0.2f));
        }
    }

    private void Die()
    {
        Debug.Log("EnemyDamageTest died!");
        Destroy(gameObject);
    }

    private IEnumerator FlashWhite(float duration)
    {
        if (spriteRenderer != null)
        {
            Color flashColor = Color.red;
            spriteRenderer.color = flashColor;
            yield return new WaitForSeconds(duration);
            spriteRenderer.color = originalColor;
        }
    }
}
