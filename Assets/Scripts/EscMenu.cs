using System.Collections.Generic;
using System.Linq;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class EscMenu : MonoBehaviour
{
    GameManager gameManager;
    AudioSystem audioSystem;

    // Accessible UI
    [SerializeField] GameObject[] selectableObjects;
    [SerializeField] AudioClip[] selectableNarrator;
    Dictionary<GameObject, AudioClip> selectables;
    InputAction keyUpArrow;
    InputAction keyDownArrow;
    InputAction keyLeftArrow;
    InputAction keyRightArrow;
    InputAction keyEnter;
    int selectedIndex;

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
        gameManager.SetVolume(AudioSystem.SOUND_TYPE.VOICE, value);
    }
    public void OnGameVolumeValueChange(float value)
    {
        gameManager.SetVolume(AudioSystem.SOUND_TYPE.GAME, value);
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
        audioSystem.Stop();
    }
    public void Select(int index, bool silent)
    {
        GameObject selected = selectables.ElementAt(selectedIndex).Key;
        selected.transform.GetChild(0).gameObject.SetActive(false);
        selected = selectables.ElementAt(index).Key;
        selectedIndex = index;
        selected.transform.GetChild(0).gameObject.SetActive(true);
        if (gameManager.narratorOn && !silent)
        {
            audioSystem.PlaySound(selectables.ElementAt(index).Value, AudioSystem.SOUND_TYPE.NARRATOR);
        }
    }
    public void OnEnterDown()
    {
        if (selectedIndex == 3)
        {
            toggleNarrator.IsOn = !toggleNarrator.IsOn;
        }
        else if (selectedIndex == 5)
        {
            OnQuitButtonPress();
        }
    }
    public void Awake()
    {
        keyUpArrow = InputSystem.actions.FindAction("UpArrow");
        keyDownArrow = InputSystem.actions.FindAction("DownArrow");
        keyLeftArrow = InputSystem.actions.FindAction("LeftArrow");
        keyRightArrow = InputSystem.actions.FindAction("RightArrow");
        keyEnter = InputSystem.actions.FindAction("Enter");

        // Assign dictionary
        selectables = new Dictionary<GameObject, AudioClip>();
        for (int i = 0; i < selectableObjects.Length; i++)
        {
            selectables[selectableObjects[i]] = selectableNarrator[i];
        }
    }
    public void Start()
    {
        gameManager = GameManager.Instance;
        audioSystem = AudioSystem.Instance;
        masterVolume.value = gameManager.masterVolume;
        voiceVolume.value = gameManager.GetVolume(AudioSystem.SOUND_TYPE.VOICE);
        gameVolume.value = gameManager.GetVolume(AudioSystem.SOUND_TYPE.GAME);
        toggleNarrator.IsOn = gameManager.narratorOn;
        narratorSpeed.value = gameManager.narratorSpeed;
        Select(0, false);
    }
    public void Update()
    {
        if (keyUpArrow.WasPressedThisFrame())
        {
            if (selectedIndex > 0)
            {
                Select(selectedIndex - 1, false);
            }
        }
        if (keyDownArrow.WasPressedThisFrame())
        {
            if (selectedIndex < selectables.Count - 1)
            {
                Select(selectedIndex + 1, false);
            }
        }
        if (keyLeftArrow.IsPressed())
        {
            if (selectedIndex == 0)
            {
                masterVolume.value -= Time.deltaTime;
            }
            else if (selectedIndex == 1)
            {
                voiceVolume.value -= Time.deltaTime;
            }
            else if (selectedIndex == 2)
            {
                gameVolume.value -= Time.deltaTime;
            }
            else if (selectedIndex == 4)
            {
                narratorSpeed.value -= Time.deltaTime*2;
            }
        }
        if (keyRightArrow.IsPressed())
        {
            if (selectedIndex == 0)
            {
                masterVolume.value += Time.deltaTime;
            }
            else if (selectedIndex == 1)
            {
                voiceVolume.value += Time.deltaTime;
            }
            else if (selectedIndex == 2)
            {
                gameVolume.value += Time.deltaTime;
            }
            else if (selectedIndex == 4)
            {
                narratorSpeed.value += Time.deltaTime*2;
            }
        }
        if (keyEnter.WasPressedThisFrame())
        {
            OnEnterDown();
        }

    }
}
