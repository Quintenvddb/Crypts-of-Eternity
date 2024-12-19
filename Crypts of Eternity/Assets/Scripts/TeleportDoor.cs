using System.Collections;
using UnityEngine;

public class TeleportDoor : MonoBehaviour
{
    public Vector3 targetPosition; // Target position where the player will be teleported
    public string playerTag = "Player"; // Tag to identify the player
    private Coroutine teleportCoroutine; // Reference to the coroutine
    private bool playerInBounds = false; // Tracks if the player is still in the bounds

    // Method to set the teleport target dynamically
    public void SetTargetPosition(Vector3 newPosition)
    {
        targetPosition = newPosition;
        Debug.Log("Teleport target position set to " + targetPosition);
    }

    // Trigger the teleport process when the player enters the door's area
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag(playerTag))
        {
            playerInBounds = true; // Mark the player as inside bounds
            teleportCoroutine = StartCoroutine(TeleportAfterDelay(other.gameObject, 3f));
        }
    }

    // Stop the teleport process if the player exits the door's area
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag(playerTag))
        {
            playerInBounds = false; // Mark the player as outside bounds
            if (teleportCoroutine != null)
            {
                StopCoroutine(teleportCoroutine);
                teleportCoroutine = null;
                Debug.Log("Player left the door's area. Teleportation canceled.");
            }
        }
    }

    // Coroutine to teleport the player after a delay
    private IEnumerator TeleportAfterDelay(GameObject player, float delay)
    {
        Debug.Log("Player detected. Waiting for " + delay + " seconds...");
        yield return new WaitForSeconds(delay);

        // Teleport the player if they're still marked as in bounds
        if (playerInBounds)
        {
            TeleportPlayer(player);
        }
        else
        {
            Debug.Log("Player moved out of the door's area. Teleportation not performed.");
        }
    }

    // Teleport the player to the target position
    private void TeleportPlayer(GameObject player)
    {
        player.transform.position = targetPosition;
        Debug.Log("Player Teleported to " + targetPosition);
    }
}
