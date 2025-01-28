using UnityEngine;

public class CoinPickupController : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Player")) return;

        PlayerController playerController = collision.GetComponent<PlayerController>();
        if (playerController == null) return;

        playerController.AddCoins();
        Destroy(gameObject);
    }
}
