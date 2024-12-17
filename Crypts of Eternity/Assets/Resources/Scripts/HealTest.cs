using UnityEngine;

public class HealTest : MonoBehaviour
{
    public int healAmount = 10;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            PlayerController player = Object.FindFirstObjectByType<PlayerController>();
            if (player != null)
            {
                player.Heal(healAmount);
            }
        }
    }
}