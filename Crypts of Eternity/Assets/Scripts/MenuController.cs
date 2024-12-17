using UnityEngine;
using UnityEngine.UI;

public class MenuController : MonoBehaviour
{
    public GameObject menu;
    public Button quitButton;

    void Start()
    {
        quitButton.onClick.AddListener(QuitGame);
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
}
