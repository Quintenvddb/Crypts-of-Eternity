using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class DeathMenuController : MonoBehaviour
{
    public GameObject deathMenu;
    public Button quitButton;
    public Button respawnButton;

    void Start()
    {
        quitButton.onClick.AddListener(QuitGame);
        respawnButton.onClick.AddListener(RestartGame);
    }

    void PlayerDied()
    {
        deathMenu.SetActive(true);
        Time.timeScale = 0;
    }

    void QuitGame()
    {
        Application.Quit();
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#endif
    }

    void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        deathMenu.SetActive(false);
        Time.timeScale = 1;
        
    }
}
