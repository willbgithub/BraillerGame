using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using System.Linq;

public class MainMenu : MonoBehaviour
{
    GameManager gameManager;
    AudioSystem audioSystem;

    InputAction keyUpArrow;
    InputAction keyDownArrow;
    InputAction keyEnter;
    [SerializeField] GameObject image;
    [SerializeField] GameObject escMenu;

    // Accessible UI
    [SerializeField] GameObject[] selectableObjects;
    [SerializeField] AudioClip[] selectableNarrator;
    Dictionary<GameObject, AudioClip> selectables;

    public void OnBraillerTutorialButtonPress()
    {
        SceneManager.LoadScene("Tutorial");
        Select(0, true);
    }
    public void OnPaperButtonPress()
    {
        SceneManager.LoadScene("PaperTest");
        Select(1, true);
    }
    public void OnCopyGameButtonPress()
    {
        SceneManager.LoadScene("CopyGame");
        Select(2, true);
    }
    public void SetEscMenu(bool value)
    {
        if (value)
        {
            Select(3, true);
        }
        escMenu.SetActive(value);
    }
    public void OnQuitButtonPress()
    {
        Application.Quit();
    }
    public void Select(int index, bool silent)
    {
        GameObject selected = selectables.ElementAt(gameManager.menuIndex).Key;
        selected.transform.GetChild(0).gameObject.SetActive(false);
        gameManager.menuIndex = index;
        selected = selectables.ElementAt(gameManager.menuIndex).Key;
        selected.transform.GetChild(0).gameObject.SetActive(true);
        if (gameManager.narratorOn && !silent)
        {
            audioSystem.PlaySound(selectables.ElementAt(gameManager.menuIndex).Value, AudioSystem.SOUND_TYPE.NARRATOR);
        }
    }
    public void OnEnterDown()
    {
        if (gameManager.menuIndex == 0)
        {
            OnBraillerTutorialButtonPress();
        }
        else if (gameManager.menuIndex == 1)
        {
            OnPaperButtonPress();
        }
        else if (gameManager.menuIndex == 2)
        {
            OnCopyGameButtonPress();
        }
        else if (gameManager.menuIndex == 3)
        {
            SetEscMenu(true);
        }
        else if (gameManager.menuIndex == 4)
        {
            OnQuitButtonPress();
        }
    }
    void Awake()
    {
        keyUpArrow = InputSystem.actions.FindAction("UpArrow");
        keyDownArrow = InputSystem.actions.FindAction("DownArrow");
        keyEnter = InputSystem.actions.FindAction("Enter");

        // Assign dictionary
        selectables = new Dictionary<GameObject, AudioClip>();
        for (int i = 0; i < selectableObjects.Length; i++)
        {
            selectables[selectableObjects[i]] = selectableNarrator[i];
        }
    }
    void Start()
    {
        gameManager = GameManager.Instance;
        audioSystem = AudioSystem.Instance;

        Select(gameManager.menuIndex, false);
        SetEscMenu(false);
    }
    void Update()
    {
        if (keyUpArrow.WasPressedThisFrame() && !escMenu.activeSelf)
        {
            if (gameManager.menuIndex > 0)
            {
                Select(gameManager.menuIndex - 1, false);
            }
        }
        if (keyDownArrow.WasPressedThisFrame() && !escMenu.activeSelf)
        {
            if (gameManager.menuIndex < selectables.Count - 1)
            {
                Select(gameManager.menuIndex + 1, false);
            }
        }
        if (keyEnter.WasPressedThisFrame() && !escMenu.activeSelf)
        {
            OnEnterDown();
        }
    }
}
