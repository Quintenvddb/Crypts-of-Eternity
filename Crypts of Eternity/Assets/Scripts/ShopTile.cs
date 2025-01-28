using UnityEngine;

public class ShopTile : MonoBehaviour
{
    private InGameUIController inGameUIController;

    private void Start()
    {
        inGameUIController = Object.FindFirstObjectByType<InGameUIController>();
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.CompareTag("Player"))
        {
            inGameUIController.OpenShop();
        }
    }
}
