using UnityEngine;
using UnityEngine.UI;

public class MenuController : MonoBehaviour
{
    public GameObject menu;
    public Button quitButton;
    public Button fullscreenToggleButton;

    private bool isFullscreen = true;

    void Start()
    {
        quitButton.onClick.AddListener(QuitGame);
        fullscreenToggleButton.onClick.AddListener(ToggleFullscreen);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            bool isMenuActive = !menu.activeSelf;
            menu.SetActive(isMenuActive);

            Time.timeScale = isMenuActive ? 0 : 1;
        }
    }

    void QuitGame()
    {
        Application.Quit();
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #endif
    }

    void ToggleFullscreen()
    {
        isFullscreen = !isFullscreen;
        Screen.fullScreenMode = isFullscreen ? FullScreenMode.FullScreenWindow : FullScreenMode.Windowed;
        Screen.fullScreen = isFullscreen;
    }
}
