using UnityEngine;
using UnityEngine.UI;
using Unity.Cinemachine;
using UnityEngine.SceneManagement;

public class BossFightController : MonoBehaviour
{
    public GameObject bossPrefab;
    public Vector3 spawnPosition = new Vector3(89, 0, -10);
    public CinemachineCamera cinemachineCamera;
    public AudioSource audioSource;
    public AudioSource audioSourceToMute;
    private float zoomOutSize = 6.5f;
    private float zoomSpeed = 1f;
    private float originalSize;
    public GameObject bossDefeatedUI;
    public Button quitButton;
    public Button respawnButton;

    private void Start()
    {
        if (cinemachineCamera != null)
        {
            originalSize = cinemachineCamera.Lens.OrthographicSize;
        }
        else
        {
            Debug.LogWarning("Cinemachine Camera is not assigned!");
        }

        if (audioSource == null)
        {
            Debug.LogWarning("AudioSource is not assigned!");
        }

        if (audioSourceToMute == null)
        {
            Debug.LogWarning("AudioSource to mute is not assigned!");
        }
        quitButton.onClick.AddListener(QuitGame);
        respawnButton.onClick.AddListener(RestartGame);
    }

    public void SpawnBoss()
    {
        StartCoroutine(SpawnBossWithZoom());
    }

    private System.Collections.IEnumerator SpawnBossWithZoom()
    {
        yield return new WaitForSeconds(1f);

        ZoomOut();
        PlayBossMusic();
        MuteOtherAudioSource();

        yield return new WaitForSeconds(2f);

        if (bossPrefab != null)
        {
            Instantiate(bossPrefab, spawnPosition, Quaternion.identity);
            Debug.Log("Prefab spawned at " + spawnPosition);
        }
        else
        {
            Debug.LogWarning("Prefab to spawn is not assigned!");
        }
    }

    private void ZoomOut()
    {
        if (cinemachineCamera != null)
        {
            StartCoroutine(ZoomTo(zoomOutSize));
        }
    }

    private void PlayBossMusic()
    {
        if (audioSource != null && !audioSource.isPlaying)
        {
            audioSource.loop = true;
            audioSource.Play();
        }
    }

    private void MuteOtherAudioSource()
    {
        if (audioSourceToMute != null)
        {
            audioSourceToMute.mute = true;
        }
    }

    private System.Collections.IEnumerator ZoomTo(float targetSize)
    {
        float currentSize = cinemachineCamera.Lens.OrthographicSize;
        while (Mathf.Abs(currentSize - targetSize) > 0.01f)
        {
            currentSize = Mathf.Lerp(currentSize, targetSize, zoomSpeed * Time.deltaTime);
            cinemachineCamera.Lens.OrthographicSize = currentSize;
            yield return null;
        }

        cinemachineCamera.Lens.OrthographicSize = targetSize;
    }

    public void BossDefeated()
    {
        bossDefeatedUI.SetActive(true);
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
        Time.timeScale = 1;

    }
}
