using UnityEngine;

public class Trap : MonoBehaviour
{
    public int damage = 5;
    public float damageCooldown = 1f;
    private float lastDamageTime = 0f;
    private Transform player;

    public void Start()
    {
        GameObject playerObject = GameObject.FindWithTag("Player");
        if (playerObject != null)
        {
            player = playerObject.transform;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            TriggerTrap(collision);
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            TriggerTrap(collision);
        }
    }

    private void TriggerTrap(Collider2D collision)
    {
            if (Time.time - lastDamageTime >= damageCooldown)
            {
                PlayerController player = collision.GetComponent<PlayerController>();
                if (player != null)
                {
                    player.TakeDamage(damage);
                }

                lastDamageTime = Time.time;
            }
    }
}