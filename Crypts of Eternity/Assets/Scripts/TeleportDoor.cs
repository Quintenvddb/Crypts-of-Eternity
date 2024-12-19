using UnityEngine;

public class TeleportDoor : MonoBehaviour
{
    public Vector3 targetPosition = new Vector3(400, 400, 0); // Target position where the player will be teleported
    public string playerTag = "Player"; // Tag to identify the player

    // Optional: You can use a collider to trigger the teleport when the player enters the door's area
    private void OnTriggerEnter2D(Collider2D other)
    {
        // Check if the object entering the trigger is the player
        if (other.CompareTag(playerTag))
        {
            TeleportPlayer(other.gameObject);
        }
    }

    // Teleport the player to the target position
    private void TeleportPlayer(GameObject player)
    {
        player.transform.position = targetPosition;
        Debug.Log("Player Teleported to " + targetPosition);
    }
}