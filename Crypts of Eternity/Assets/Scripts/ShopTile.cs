using UnityEngine;
using UnityEngine.UI;

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
        Debug.Log("Shop has been opened");
        Time.timeScale = 0;
    }

    public void CloseShop()
    {
        if (shopUI == null)
    {
        Debug.LogError("shopUI is not assigned in the Inspector!");
        return;
    }

    shopUI.SetActive(false);
    foreach (Transform child in shopUI.transform)
    {
        child.gameObject.SetActive(false);
    }

    Debug.Log("Shop has been closed");
    Time.timeScale = 1;
    }
}
