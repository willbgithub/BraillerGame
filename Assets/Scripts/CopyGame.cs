using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ReactionGame : MonoBehaviour, IBrailler
{
    GameManager gameManager;
    AudioSystem audioSystem;

    // Data
    int score = 0;
    int totalWrites = 0;
    InputAction keyGuide;

    InputAction keyUpArrow;
    InputAction keyDownArrow;
    InputAction keyEnter;
    InputAction keyEsc;

    // Accessible UI
    [SerializeField] GameObject[] selectableObjects;
    [SerializeField] AudioClip[] selectableNarrator;
    Dictionary<GameObject, AudioClip> selectables;
    int selectedIndex;

    bool audioQueuePlaying = false;
    float audioQueueTimer = 0;

    // Custom Parameters
    [SerializeField] Color colorDefault;
    [SerializeField] Color colorCorrect;
    [SerializeField] Color colorWrong;
    // Serialized Objects
    [SerializeField] GameObject guide;
    [SerializeField] GameObject escMenu;
    [SerializeField] Toggle toggleLetterMode;
    [SerializeField] Toggle toggleShowCopyCell;
    [SerializeField] Brailler brailler;
    [SerializeField] BrailleCell brailleCellInput;
    [SerializeField] BrailleCell brailleCellCopy;
    [SerializeField] TMP_Text labelFeedback;
    [SerializeField] TMP_Text labelScore;
    [SerializeField] TMP_Text labelInputLetter;
    [SerializeField] TMP_Text labelCopyLetter;
    [SerializeField] AudioClip audioCorrect;
    [SerializeField] AudioClip audioWrong;


    public void OnBackButtonPress()
    {
        SceneManager.LoadScene("MainMenu");
    }
    // Game
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
    public void ShowCopyCell(bool value)
    {
        brailleCellCopy.Hide(!value);
    }
    public void Score(bool scored)
    {
        totalWrites++;
        if (scored)
        {
            score++;
        }
        labelScore.text = $"Score: {(int)(100f*score/totalWrites)}%";
    }
    IEnumerator StartRound(float secDelay = 0)
    {
        if (secDelay > 0)
        {
            yield return new WaitForSeconds(secDelay);
        }

        brailleCellInput.SetBackgroundColor(colorDefault);
        char character = (char)('a' + Random.Range(0, 25));
        while (character == brailleCellCopy.GetCharacter())
        {
            character = (char)('a' + Random.Range(0, 25));
        }
        brailleCellCopy.SetCharacter(character);
        labelCopyLetter.text = character.ToString();
        brailleCellInput.SetIndex(0);
        labelInputLetter.text = "";
        labelFeedback.text = "COPY!";
        StartCoroutine(PlayBrailleAudio(brailleCellCopy.GetIndex()));
    }
    IEnumerator PlayBrailleAudio(int index, float secDelay = 0)
    {
        if (secDelay > 0)
        {
            yield return new WaitForSeconds(secDelay);
        }

        // Character
        if (toggleLetterMode.IsOn)
        {
            audioSystem.PlayCharacterAudio(brailleCellCopy.GetCharacter());
            brailler.SetOutputEnabled(true);
            yield break;
        }
        brailler.SetOutputEnabled(false);
        // Dots
        bool[] dots = new bool[6];
        for (int i = 5; i >= 0; i--)
        {
            if (index >= (int)Mathf.Pow(2, i))
            {
                dots[i] = true;
                index -= (int)Mathf.Pow(2, i);
            }
            else
            {
                dots[i] = false;
            }
        }
        List<char> characters = new List<char>();
        for (int i = 0; i < 6; i++)
        {
            if (dots[i])
            {
                characters.Add((char)('1'+i));
            }
        }
        audioQueuePlaying = true;
        audioQueueTimer = audioSystem.PlayCharacterQueue(characters);
    }

    // Input
    public void OnIndexWrite(int index)
    {
        brailler.SetOutputEnabled(false);
        brailleCellInput.SetIndex(index);
        if (brailleCellInput.HasCharacter())
        {
            char character = brailleCellInput.GetCharacter();
            labelInputLetter.text = character.ToString();
        }
        else
        {
            labelInputLetter.text = "Not a character!";
        }
        if (index == brailleCellCopy.GetIndex())
        {
            labelFeedback.text = "CORRECT!";
            brailleCellInput.SetBackgroundColor(colorCorrect);
            audioSystem.PlaySound(audioCorrect, 0.45f, 1f, AudioSystem.SOUND_TYPE.GAME);
            Score(true);
            StartCoroutine(StartRound(1));
        }
        else
        {
            labelFeedback.text = "WRONG!";
            brailleCellInput.SetBackgroundColor(colorWrong);
            audioSystem.PlaySound(audioWrong, AudioSystem.SOUND_TYPE.GAME);
            Score(false);
            StartCoroutine(PlayBrailleAudio(brailleCellCopy.GetIndex(), 1));
        }
        
    }
    public void OnAudioQueueEnd()
    {
        brailler.SetOutputEnabled(true);
    }
    public void OnBackspaceDown()
    {

    }
    public void OnSpaceDown()
    {
        StartCoroutine(PlayBrailleAudio(brailleCellCopy.GetIndex()));
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
        if (selectedIndex == 0)
        {
            toggleLetterMode.IsOn = !toggleLetterMode.IsOn;
        }
        else if (selectedIndex == 1)
        {
            toggleShowCopyCell.IsOn = !toggleShowCopyCell.IsOn;
        }
    }

    // Other
    void Awake()
    {
        brailler.SetParent(this);
        keyUpArrow = InputSystem.actions.FindAction("UpArrow");
        keyDownArrow = InputSystem.actions.FindAction("DownArrow");
        keyEnter = InputSystem.actions.FindAction("Enter");
        keyGuide = InputSystem.actions.FindAction("Guide");
        keyEsc = InputSystem.actions.FindAction("Esc");

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
        
        Select(0, false);
        SetEscMenu(false);
        SetGuide(false);
        brailleCellCopy.SetBackgroundColor(colorDefault);
        ShowCopyCell(toggleShowCopyCell.IsOn);
        StartCoroutine(StartRound(0));
    }

    void Update()
    {
        // Audio Queue
        if (audioQueueTimer > 0)
        {
            audioQueueTimer -= Time.deltaTime;
        }
        else if (audioQueuePlaying)
        {
            audioQueuePlaying = false;
            OnAudioQueueEnd();
        }

        // Controls
        if (keyUpArrow.WasPressedThisFrame() && !escMenu.activeSelf)
        {
            if (selectedIndex > 0)
            {
                Select(selectedIndex - 1, false);
            }
        }
        if (keyDownArrow.WasPressedThisFrame() && !escMenu.activeSelf)
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

        // Guide
        if (keyGuide.WasPressedThisFrame())
        {
            ToggleGuide();
        }

        if (keyEsc.WasPressedThisFrame())
        {
            ToggleEscMenu();
        }
    }
}
