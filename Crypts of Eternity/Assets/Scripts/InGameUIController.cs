using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class InGameUIController : MonoBehaviour
{
    public Text coinText;
    public Text healthText;
    public GameObject shopUI;
    private PlayerController playerController;
    public Button closeShopButton;
    public AudioSource audioSource;
    public AudioClip backgroundAudio;
    public float backgroundVolume = 1.0f;

    void Start()
    {
        playerController = FindFirstObjectByType<PlayerController>();
        StartCoroutine(UpdateUI());

        closeShopButton.onClick.AddListener(CloseShop);
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

    public void OpenShop()
    {
        shopUI.SetActive(true);
        audioSource.mute = true;
        Time.timeScale = 0;
    }

    public void CloseShop()
    {
        shopUI.SetActive(false);
        audioSource.mute = false;
        Time.timeScale = 1;
    }
}
