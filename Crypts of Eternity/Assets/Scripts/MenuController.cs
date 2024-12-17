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
            menu.SetActive(!menu.activeSelf);
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
