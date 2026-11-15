using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class EscMenu : MonoBehaviour
{
    public GameManager gameManager;

    [SerializeField] Slider masterVolume;
    [SerializeField] Slider voiceVolume;
    [SerializeField] Slider gameVolume;
    [SerializeField] Toggle toggleNarrator;
    [SerializeField] Button quitButton;
    public void Awake()
    {
        gameManager = GameManager.Instance;

        masterVolume.value = gameManager.masterVolume;
        voiceVolume.value = gameManager.voiceVolume;
        gameVolume.value = gameManager.gameVolume;
        toggleNarrator.IsOn = gameManager.narratorOn;
    }
    public void OnMasterVolumeValueChange(float value)
    {
        Debug.Log($"OnMasterVolumeValueChange({value})");
        Debug.Log($"gameManager = \"{gameManager}\"");
        Debug.Log($"gameManager.masterVolume = \"{gameManager.masterVolume}\"");
        gameManager.masterVolume = value;
    }
    public void OnVoiceVolumeValueChange(float value)
    {
        gameManager.voiceVolume = value;
    }
    public void OnGameVolumeValueChange(float value)
    {
        gameManager.gameVolume = value;
    }
    public void OnToggleNarrator(bool value)
    {
        gameManager.narratorOn = value;
    }
    public void OnQuitButtonPress()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
