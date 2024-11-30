using UnityEngine;
using UnityEngine.UI;

public class HealthSliderController : MonoBehaviour
{
    public Slider healthSlider;
    private PlayerController playerController;

    void Start()
    {
        playerController = Object.FindFirstObjectByType<PlayerController>();

        if (playerController != null && healthSlider != null)
        {
            healthSlider.value = GetHealthPercentage();
        }
    }

    void Update()
    {
        if (playerController != null && healthSlider != null)
        {
            float currentHealthPercentage = GetHealthPercentage();
            if (!Mathf.Approximately(healthSlider.value, currentHealthPercentage))
            {
                healthSlider.value = currentHealthPercentage;
            }
        }
    }

    private float GetHealthPercentage()
    {
        if (playerController != null)
        {
            return (float)playerController.GetCurrentHealth() / playerController.maxHealth;
        }
        return 0f;
    }
}
