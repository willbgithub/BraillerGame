using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Game : MonoBehaviour, IBrailler
{
    GameManager gameManager;
    AudioSystem audioSystem;

    // Data
    // Accessible UI
    [SerializeField] GameObject[] selectableObjects;
    [SerializeField] AudioClip[] selectableNarrator;
    Dictionary<GameObject, AudioClip> selectables;
    int selectedIndex = 0;
    bool tutorialOpen = false;

    int scrollToBottom = 0;
        
    InputAction keyGuide;

    InputAction keyUpArrow;
    InputAction keyDownArrow;
    InputAction keyEnter;
    InputAction keyEsc;

    // Serialized objects
    [SerializeField] Brailler brailler;
    [SerializeField] BrailleCell brailleCellPrefab;
    [SerializeField] Transform paperContent;
    [SerializeField] GameObject guide;
    [SerializeField] GameObject tutorial;
    [SerializeField] GameObject escMenu;
    [SerializeField] ScrollRect scrollView;
    [SerializeField] ScrollRect textScrollView;
    [SerializeField] TMP_Text textLabel;
    [SerializeField] Toggle audioToggle;

    [SerializeField] AudioClip testSound;
    [SerializeField] AudioClip audioTutorial;

    public void SetAudioEnabled(bool value)
    {
        gameManager.PaperTestAudioOn = value;
    }
    public void SetEscMenu(bool value)
    {
        escMenu.SetActive(value);
    }
    public void ToggleEscMenu()
    {
        SetEscMenu(!escMenu.activeSelf);
    }
    public void SetGuide(bool guide)
    {
        this.guide.SetActive(guide);
    }
    public void ToggleGuide()
    {
        SetGuide(!guide.activeSelf);
    }
    public void OnIndexWrite(int index)
    {
        CreateBrailleCell(index);
        if (audioToggle.IsOn && Braille.ContainsIndex(index))
        {
            audioSystem.PlayCharacterAudio(Braille.GetCharacter(index));
        }
    }
    public void OnBackspaceDown()
    {
        int childCount = paperContent.childCount;
        if (childCount > 0)
        {
            BrailleCell cell = paperContent.GetChild(childCount - 1).GetComponent<BrailleCell>();
            if (cell.HasCharacter())
            {
                textLabel.text = textLabel.text.Substring(0, textLabel.text.Length - 1);
            }
            Destroy(cell.gameObject);
        }
    }
    public void OnSpaceDown()
    {
        CreateBrailleCell(0);
    }
    // Index of 0 is a space
    public void CreateBrailleCell(int index)
    {
        BrailleCell brailleCell = Instantiate(brailleCellPrefab, paperContent);
        brailleCell.SetIndex(index);
        if (brailleCell.HasCharacter())
        {
            textLabel.text += brailleCell.GetCharacter();
        }
        scrollToBottom = 3;
    }
    public void OnClearButtonPress()
    {
        textLabel.text = "";
        int childCount = paperContent.childCount;
        for (int i = 0; i < childCount; i++)
        {
            Destroy(paperContent.GetChild(i).gameObject);
        }
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
        if (tutorialOpen)
        {
            ShowTutorial(false);
            return;
        }
        if (selectedIndex == 0)
        {
            ShowTutorial(true);
        }
        else if (selectedIndex == 1)
        {
            OnClearButtonPress();
        }
        else if (selectedIndex == 2)
        {
            audioToggle.ToggleValue();
        }
    }
    public void ShowTutorial(bool value)
    {
        tutorialOpen = value;
        tutorial.SetActive(value);
        if (value)
        {
            selectables.ElementAt(selectedIndex).Key.transform.GetChild(0).gameObject.SetActive(false);
            audioSystem.PlaySound(audioTutorial, AudioSystem.SOUND_TYPE.NARRATOR);
        }
        else
        {
            if (!gameManager.PaperTestTutorialSeen)
            {
                gameManager.PaperTestTutorialSeen = true;
            }
            Select(selectedIndex, false);
        }
    }
    void Awake()
    {
        brailler.SetParent(this);
        keyUpArrow = InputSystem.actions.FindAction("UpArrow");
        keyDownArrow = InputSystem.actions.FindAction("DownArrow");
        keyEnter = InputSystem.actions.FindAction("Enter");
        keyGuide = InputSystem.actions.FindAction("Guide");
        keyEsc = InputSystem.actions.FindAction("Esc");
        textLabel.text = "";

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
        audioToggle.IsOn = gameManager.PaperTestAudioOn;
        if (gameManager.PaperTestTutorialSeen)
        {
            ShowTutorial(false);
            Select(0, false);
        }
        else
        {
            ShowTutorial(true);
        }
        SetGuide(false);
        SetEscMenu(false);
    }
    void Update()
    {
        if (keyGuide.WasPressedThisFrame())
        {
            ToggleGuide();
        }
        if (keyUpArrow.WasPressedThisFrame() && !escMenu.activeSelf && !tutorialOpen)
        {
            if (selectedIndex > 0)
            {
                Select(selectedIndex - 1, false);
            }
        }
        if (keyDownArrow.WasPressedThisFrame() && !escMenu.activeSelf && !tutorialOpen)
        {
            if (selectedIndex < selectables.Count - 1)
            {
                Select(selectedIndex + 1, false);
            }
        }
        if (keyEnter.WasPressedThisFrame() && !escMenu.activeSelf)
        {
            OnEnterDown();
        }
        if (scrollToBottom >= 1)
        {
            scrollToBottom--;
            scrollView.verticalScrollbar.value = 0;
            textScrollView.verticalScrollbar.value = 0;
        }
        if (keyEsc.WasPressedThisFrame())
        {
            ToggleEscMenu();
        }
    }
}
