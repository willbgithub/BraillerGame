using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class EscMenu : MonoBehaviour
{
    GameManager gameManager;

    [SerializeField] Slider masterVolume;
    [SerializeField] Slider voiceVolume;
    [SerializeField] Slider gameVolume;
    [SerializeField] Toggle toggleNarrator;
    [SerializeField] Slider narratorSpeed;
    [SerializeField] Button quitButton;
    public void OnMasterVolumeValueChange(float value)
    {
        gameManager.masterVolume = value;
    }
    public void OnVoiceVolumeValueChange(float value)
    {
        gameManager.soundSettings[AudioSystem.SOUND_TYPE.VOICE] = value;
    }
    public void OnGameVolumeValueChange(float value)
    {
        gameManager.soundSettings[AudioSystem.SOUND_TYPE.GAME] = value;
    }
    public void OnToggleNarrator(bool value)
    {
        gameManager.narratorOn = value;
    }
    public void OnNarratorSpeedValueChange(float value)
    {
        gameManager.narratorSpeed = value;
    }
    public void OnQuitButtonPress()
    {
        SceneManager.LoadScene("MainMenu");
    }
    public void Awake()
    {
        
    }
    public void Start()
    {
        gameManager = GameManager.Instance;
        masterVolume.value = gameManager.masterVolume;
        voiceVolume.value = gameManager.soundSettings[AudioSystem.SOUND_TYPE.VOICE];
        gameVolume.value = gameManager.soundSettings[AudioSystem.SOUND_TYPE.GAME];
        toggleNarrator.IsOn = gameManager.narratorOn;
        narratorSpeed.value = gameManager.narratorSpeed;
    }
    public void Update()
    {
        
    }
}
