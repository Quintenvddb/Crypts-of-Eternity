using UnityEngine;
using TMPro;
using UnityEngine.Profiling;

public class ProfilerScript : MonoBehaviour
{
    public TMP_Text fpsText;
    public TMP_Text cpuText;
    public TMP_Text gpuText;
    public TMP_Text ramText;
    private float deltaTime = 0.0f;
    private float timer = 0.0f;
    private int frameCount = 0;

    private float cpuTime;
    private float gpuTime;
    private float ramUsage;

    private bool statsEnabled = false;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            statsEnabled = !statsEnabled;
        }

        if (statsEnabled)
        {
            deltaTime += (Time.unscaledDeltaTime - deltaTime) * 0.1f;
            frameCount++;

            timer += Time.unscaledDeltaTime;
            if (timer >= 1.0f)
            {
                float fps = frameCount / timer;
                if (fpsText != null)
                {
                    fpsText.text = $"FPS: {Mathf.Ceil(fps)}";
                }

                Profiler.BeginSample("CPUFrameTime");
                cpuTime = Time.unscaledDeltaTime * 1000f;
                Profiler.EndSample();

                if (cpuText != null)
                {
                    cpuText.text = $"CPU: {cpuTime:F4} ms";
                }

                Profiler.BeginSample("GPUFrameTime");
                gpuTime = Time.smoothDeltaTime * 1000f;
                Profiler.EndSample();

                if (gpuText != null)
                {
                    gpuText.text = $"GPU: {gpuTime:F4} ms";
                }

                ramUsage = System.GC.GetTotalMemory(false) / 1024f / 1024f;
                if (ramText != null)
                {
                    ramText.text = $"RAM: {ramUsage:F4} MB";
                }

                timer = 0.0f;
                frameCount = 0;
            }
        }
        else
        {
            if (fpsText != null) fpsText.text = string.Empty;
            if (cpuText != null) cpuText.text = string.Empty;
            if (gpuText != null) gpuText.text = string.Empty;
            if (ramText != null) ramText.text = string.Empty;
        }
    }
}
