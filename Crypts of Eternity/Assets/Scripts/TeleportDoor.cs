using System.Collections;
using UnityEngine;
using UnityEngine.UI;  // Include for Text references (use TMPro if you're using TextMeshPro)

public class TeleportDoor : MonoBehaviour
{
    public Vector3 targetPosition; // Target position where the player will be teleported
    public string playerTag = "Player"; // Tag to identify the player
    public GameObject circle; // The expanding circle child object
    public AudioSource doorCreakSound; // AudioSource for the door creaking sound
    private Coroutine teleportCoroutine; // Reference to the teleport coroutine
    private Coroutine circleCoroutine; // Reference to the circle animation coroutine
    private bool playerInBounds = false; // Tracks if the player is still in bounds
    private Vector3 originalCircleScale; // Original scale of the circle
    private bool isTeleporting = false; // Tracks whether teleportation is currently happening
    private PlayerController playerController; // Declare the reference
    // Pop-up UI references
    public GameObject NoKeyPopup;  // Reference to the pop-up panel
    private float popUpDuration = 3f; // How long to show the pop-up
    private bool hasTeleported = false; // Tracks if the player has teleported
    public BossFightController bossFightController; // Reference to BossFightController
    public GameObject warning;


    private void Start()
    {
        playerController = Object.FindFirstObjectByType<PlayerController>(); // Find the PlayerController at runtime
        bossFightController = Object.FindFirstObjectByType<BossFightController>(); // Find the BossFightController at runtime
        warning = GameObject.Find("UI").transform.Find("WarningUI").gameObject;

        if (circle != null)
        {
            originalCircleScale = circle.transform.localScale; // Store the original scale of the circle
        }
        if (circle == null)
        {
            circle = transform.Find("Circle")?.gameObject; // Finds the circle if it's a child
            if (circle == null)
            {
                Debug.LogError("Circle object not found! Make sure it's assigned.");
            }
        }
    }

    // Method to set the teleport target dynamically
    public void SetTargetPosition(Vector3 newPosition)
    {
        targetPosition = newPosition;
        Debug.Log("Teleport target position set to " + targetPosition);
    }

    // Trigger the teleport process when the player enters the door's area
    private IEnumerator OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag(playerTag))
        {
            if (hasTeleported)
            {
                Debug.Log("Player has already teleported. Teleportation denied.");
                yield break; // Exit if the player has already teleported
            }

            playerInBounds = true; // Mark the player as inside bounds

            // Check if the player has the BossRoomKey
            if (!playerController.GetComponent<InventoryManager>().HasItem("BossRoomKey"))
            {
                Debug.Log("Player does not have the BossRoomKey. Teleportation canceled.");
                ShowPopUp(); // Show the pop-up message
                yield break; // Exit the coroutine
            }
            else
            {
                StartAnimationAndSound(); // Start the circle animation and sound
                teleportCoroutine = StartCoroutine(TeleportAfterDelay(other.gameObject, 3f));
            }
        }
    }

    // Show the pop-up with a message
    private void ShowPopUp()
    {
        NoKeyPopup.SetActive(true); // Show the pop-up panel
        // Start coroutine to hide the pop-up after a set duration
        StartCoroutine(HidePopUpAfterDelay());
    }

    // Coroutine to hide the pop-up after a set duration
    private IEnumerator HidePopUpAfterDelay()
    {
        yield return new WaitForSeconds(popUpDuration);
        NoKeyPopup.SetActive(false); // Hide the pop-up panel
    }

    // Stop the teleport process if the player exits the door's area
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag(playerTag))
        {
            playerInBounds = false; // Mark the player as outside bounds
            if (!isTeleporting)
            {
                // Stop the teleport coroutine if running
                if (teleportCoroutine != null)
                {
                    StopCoroutine(teleportCoroutine);
                    teleportCoroutine = null;
                    Debug.Log("Player left the door's area. Teleportation canceled.");
                }
                // Stop any ongoing circle animation coroutine
                if (circleCoroutine != null)
                {
                    StopCoroutine(circleCoroutine);
                    circleCoroutine = null;
                }
                ResetCircle();
            }
        }
    }

    private IEnumerator TeleportAfterDelay(GameObject player, float delay)
    {
        Debug.Log("Player detected. Waiting for " + delay + " seconds...");
        yield return new WaitForSeconds(delay);

        // Teleport the player if they're still marked as in bounds
        if (playerInBounds)
        {
            isTeleporting = true; // Start teleportation
            hasTeleported = true; // Mark the player as having teleported
            MoveDoorAndCircle();
            TeleportPlayer(player);
            StartCoroutine(ShowWarning());
            bossFightController.SpawnBoss();
            yield return StartCoroutine(ShrinkCircle());
            yield return new WaitForSeconds(1f);
            isTeleporting = false; // End teleportation
        }
        else
        {
            Debug.Log("Player moved out of the door's area. Teleportation not performed.");
        }
    }

    // Start the expanding animation and play the sound
    private void StartAnimationAndSound()
    {
        if (circle != null)
        {
            circleCoroutine = StartCoroutine(ExpandCircle());
        }
        if (doorCreakSound != null)
        {
            doorCreakSound.Play();
        }
    }

    // Coroutine to expand the circle
    private IEnumerator ExpandCircle()
    {
        float duration = 2.5f;
        float elapsedTime = 0f;
        Vector3 targetScale = new Vector3(30f, 30f, 1f); // Adjust as needed for screen fill

        while (elapsedTime < duration)
        {
            circle.transform.localScale = Vector3.Lerp(originalCircleScale, targetScale, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        circle.transform.localScale = targetScale;
    }

    // Teleport the player to the target position
    private void TeleportPlayer(GameObject player)
    {
        player.transform.position = targetPosition;
        Debug.Log("Player Teleported to " + targetPosition);
    }

    // Move the door and circle to the target position
    private void MoveDoorAndCircle()
    {
        transform.position = targetPosition;
        Debug.Log("Door and circle moved to " + targetPosition);
    }

    // Coroutine to shrink the circle
    private IEnumerator ShrinkCircle()
    {
        if (circle == null) yield break;

        float duration = 4f;
        float elapsedTime = 0f;
        Vector3 targetScale = originalCircleScale; // Shrink back to the original scale

        while (elapsedTime < duration)
        {
            if (circle == null) yield break;
            circle.transform.localScale = Vector3.Lerp(circle.transform.localScale, targetScale, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        if (circle != null) circle.transform.localScale = targetScale;
    }

    // Reset the circle's scale to its original size
    private void ResetCircle()
    {
        if (circle != null)
        {
            circle.transform.localScale = originalCircleScale;
            Debug.Log("Circle scale reset to original size.");
        }
    }

    private IEnumerator ShowWarning()
    {
        warning.SetActive(true);
        yield return new WaitForSeconds(2);
        warning.SetActive(false);
    }
}
