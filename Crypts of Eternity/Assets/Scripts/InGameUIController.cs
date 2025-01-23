using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class InGameUIController : MonoBehaviour
{
    public Text coinText;
    private PlayerController playerController;

    void Start()
    {
        playerController = FindFirstObjectByType<PlayerController>();

        StartCoroutine(UpdateCoinDisplay());
    }

    private IEnumerator UpdateCoinDisplay()
    {
        while (true)
        {
            coinText.text = playerController.coins.ToString();

            yield return new WaitForSeconds(1f);
        }
    }
}