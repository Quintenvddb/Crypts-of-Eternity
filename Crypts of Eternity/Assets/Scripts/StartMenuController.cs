using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class StartMenuController : MonoBehaviour
{
    public GameObject menu;
    public Button startButton;

    void Start()
    {
        StartCoroutine(DelayTimeScale());

        if (startButton != null)
        {
            startButton.onClick.AddListener(OnStartButtonClick);
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
}
