using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class StartMenuController : MonoBehaviour
{
    public GameObject menu;
    public Button startButton;
    public Button settingsButton;
    public GameObject settingsMenu;

    void Start()
    {
        StartCoroutine(DelayTimeScale());

        if (startButton != null)
        {
            startButton.onClick.AddListener(OnStartButtonClick);
        }

        if (settingsButton != null)
        {
            settingsButton.onClick.AddListener(OnSettingsButtonClick);
        }
    }

    private IEnumerator DelayTimeScale()
    {
        yield return new WaitForSeconds(0.5f);
        Time.timeScale = 0;
    }

    private void OnStartButtonClick()
    {
        if (menu != null)
        {
            menu.SetActive(false);
        }

        Time.timeScale = 1;
    }

    private void OnSettingsButtonClick()
    {
        if (settingsMenu != null)
        {
            settingsMenu.SetActive(true);
            Time.timeScale = 0;
        }
    }
}
