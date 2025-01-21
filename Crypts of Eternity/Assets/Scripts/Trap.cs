using UnityEngine;

public class Trap : MonoBehaviour
{
    public int damage;
    public PlayerController player;
    private bool isSteppedOn = false;


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && !isSteppedOn)
        {
            TriggerTrap();
        }
    }

    private void TriggerTrap()
    {
        isSteppedOn = false;
        player.TakeDamage(damage);
    }
}
