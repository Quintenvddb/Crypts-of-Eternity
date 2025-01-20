using UnityEngine;

public class GameManager : MonoBehaviour
{
    public bool limitFrameRate = true;

    public GameObject startMenu;

    void Start()
    {
        SetFrameRate();
        startMenu.SetActive(true);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            limitFrameRate = !limitFrameRate;
            SetFrameRate();
        }
    }

    void SetFrameRate()
    {
        if (limitFrameRate)
        {
            Application.targetFrameRate = 60;
        }
        else
        {
            Application.targetFrameRate = -1;
        }
    }
}
