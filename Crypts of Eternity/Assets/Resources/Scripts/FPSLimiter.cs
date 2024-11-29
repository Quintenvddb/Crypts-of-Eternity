using UnityEngine;

public class FrameRateManager : MonoBehaviour
{
    public bool limitFrameRate = true;

    void Start()
    {
        SetFrameRate();
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
