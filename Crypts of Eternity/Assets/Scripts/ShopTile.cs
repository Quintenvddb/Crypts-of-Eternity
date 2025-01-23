using UnityEngine;

public class ShopTile : MonoBehaviour
{
    public GameObject shopUI; 

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.CompareTag("Player"))
        {
            OpenShop();
        }
    }

    private void OpenShop()
    {
        shopUI.SetActive(true);
        Time.timeScale = 0;
    }

    public void CloseShop()
    {
        shopUI.SetActive(false);
        Time.timeScale = 1;
    }
}
