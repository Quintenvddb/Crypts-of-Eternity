using UnityEngine;

public class HealTest : MonoBehaviour
{
    public int healAmount = 10;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            ApplyHealToPlayer();
        }
    }

    private void ApplyHealToPlayer()
    {
        PlayerController player = Object.FindFirstObjectByType<PlayerController>();
        if (player != null)
        {
            player.Heal(healAmount);
        }
    }
}
