using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class InGameUIController : MonoBehaviour
{
    public Text coinText;
    public Text healthText;
    private PlayerController playerController;

    void Start()
    {
        playerController = FindFirstObjectByType<PlayerController>();
        
        StartCoroutine(UpdateUI());
    }

    private IEnumerator UpdateUI()
    {
        while (true)
        {
            coinText.text = playerController.coins.ToString();
            
            healthText.text = playerController.currentHealth + "/" + playerController.maxHealth;
            
            yield return new WaitForSeconds(1f);
        }
    }
}