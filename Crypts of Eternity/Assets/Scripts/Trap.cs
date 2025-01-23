using UnityEngine;

public class Trap : MonoBehaviour
{
    public int damage;
    public GameObject player;
    private PlayerController controller;

    public void Start()
    {
        if (player != null)
        {
            controller = player.GetComponent<PlayerController>();
            Debug.Log("Controller has been found from player gameobject");
        }
        else
        {
            Debug.Log("player has not been found");
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            TriggerTrap();
        }
    }

    private void TriggerTrap()
    {
        if (controller != null)
        {
            Debug.Log("Trap has been triggered");
            controller.TakeDamage(damage);
        }
        else 
        {
            Debug.Log("Playercontroller has not been found");
        }
    }
}
