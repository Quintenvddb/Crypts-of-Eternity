using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class SettingsMenuController : MonoBehaviour
{
    public Button rebindWButton;
    public Button rebindAButton;
    public Button rebindSButton;
    public Button rebindDButton;
    public Button closeSettingsButton;
    public Text wKeyText;
    public Text aKeyText;
    public Text sKeyText;
    public Text dKeyText;

    private Dictionary<string, KeyCode> keyBindings = new Dictionary<string, KeyCode>()
    {
        { "MoveUp", KeyCode.W },
        { "MoveLeft", KeyCode.A },
        { "MoveDown", KeyCode.S },
        { "MoveRight", KeyCode.D }
    };

    private string keyToRebind;
    public GameObject settingsMenu;

    public PlayerController playerController; 

    void Start()
    {
        UpdateKeyTexts();

        rebindWButton.onClick.AddListener(() => StartRebinding("MoveUp"));
        rebindAButton.onClick.AddListener(() => StartRebinding("MoveLeft"));
        rebindSButton.onClick.AddListener(() => StartRebinding("MoveDown"));
        rebindDButton.onClick.AddListener(() => StartRebinding("MoveRight"));

        if (closeSettingsButton != null)
        {
            closeSettingsButton.onClick.AddListener(OnBackToStartMenuClick);
        }
    }

    void Update()
    {
        if (!string.IsNullOrEmpty(keyToRebind))
        {
            foreach (KeyCode key in System.Enum.GetValues(typeof(KeyCode)))
            {
                if (Input.GetKeyDown(key))
                {
                    keyBindings[keyToRebind] = key;

                    playerController.RebindKey(keyToRebind, key); 

                    keyToRebind = null;

                    UpdateKeyTexts();

                    break;
                }
            }
        }
    }

    private void StartRebinding(string action)
    {
        keyToRebind = action;
        Debug.Log($"Rebinding {action}. Press a key...");
    }

    private void UpdateKeyTexts()
    {
        wKeyText.text = keyBindings["MoveUp"].ToString();
        aKeyText.text = keyBindings["MoveLeft"].ToString();
        sKeyText.text = keyBindings["MoveDown"].ToString();
        dKeyText.text = keyBindings["MoveRight"].ToString();
    }

    public KeyCode GetKeyForAction(string action)
    {
        return keyBindings[action];
    }

    private void OnBackToStartMenuClick()
    {
        settingsMenu.SetActive(false);
    }
}
