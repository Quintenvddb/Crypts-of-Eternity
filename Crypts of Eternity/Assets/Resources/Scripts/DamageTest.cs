using UnityEngine;

public class DamageTest : MonoBehaviour
{
    public int damageAmount = 10;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            ApplyDamageToPlayer();
        }
    }

    private void ApplyDamageToPlayer()
    {
        PlayerController player = Object.FindFirstObjectByType<PlayerController>();
        if (player != null)
        {
            player.TakeDamage(damageAmount);
        }
    }
}
