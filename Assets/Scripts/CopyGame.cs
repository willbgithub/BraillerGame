using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ReactionGame : MonoBehaviour, IBrailler
{
    GameManager gameManager;

    // Data
    int score = 0;
    int totalWrites = 0;
    InputAction keyGuide;

    InputAction keyUpArrow;
    InputAction keyDownArrow;
    InputAction keyEnter;
    InputAction keyEsc;

    [SerializeField] GameObject[] selectables;
    int selectedIndex;

    // Audio Queue
    List<AudioClip> audioQueue = new List<AudioClip>();
    bool audioQueuePlaying = false;
    const float SECONDS_BETWEEN_SOUNDS_IN_QUEUE = 0.2f;
    float delayTimer = 0;

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
    [SerializeField] AudioClip audioNewRound;
    [SerializeField] AudioClip audioCorrect;
    [SerializeField] AudioClip audioWrong;
    [SerializeField] AudioClip[] audioNumberYes;
    [SerializeField] AudioClip[] audioAlphabet;
    [SerializeField] AudioSource[] audioPlayer;


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
            PlaySound(audioAlphabet[brailleCellCopy.GetCharacter() - 'a']);
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
        for (int i = 0; i < 6; i++)
        {
            if (dots[i])
            {
                audioQueue.Add(audioNumberYes[i]);
            }
        }
        PlayAudioQueue();
    }
    public void PlayAudioQueue()
    {
        delayTimer = 0;
        audioQueuePlaying = true;
    }
    public void PlaySound(AudioClip sound)
    {
        int channel = 0;
        while (audioPlayer[channel].isPlaying)
        {
            channel++;
        }
        audioPlayer[channel].resource = sound;
        audioPlayer[channel].Play();
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
            PlaySound(audioCorrect);
            Score(true);
            StartCoroutine(StartRound(1));
        }
        else
        {
            labelFeedback.text = "WRONG!";
            brailleCellInput.SetBackgroundColor(colorWrong);
            PlaySound(audioWrong);
            Score(false);
            StartCoroutine(PlayBrailleAudio(brailleCellCopy.GetIndex(), 1));
        }
        
    }
    public void OnBackspaceDown()
    {

    }
    public void OnSpaceDown()
    {
        if (audioQueuePlaying)
        {
            return;
        }
        StartCoroutine(PlayBrailleAudio(brailleCellCopy.GetIndex()));
    }
    public void Select(int index)
    {
        GameObject selected = selectables[selectedIndex];
        selected.transform.GetChild(0).gameObject.SetActive(false);
        selectedIndex = index;
        selected = selectables[selectedIndex];
        selected.transform.GetChild(0).gameObject.SetActive(true);
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
        gameManager = GameManager.Instance;

        brailler.SetParent(this);
        keyUpArrow = InputSystem.actions.FindAction("UpArrow");
        keyDownArrow = InputSystem.actions.FindAction("DownArrow");
        keyEnter = InputSystem.actions.FindAction("Enter");
        keyGuide = InputSystem.actions.FindAction("Guide");
        keyEsc = InputSystem.actions.FindAction("Esc");
    }
    void Start()
    {
        Select(0);
        SetEscMenu(false);
        SetGuide(false);
        brailleCellCopy.SetBackgroundColor(colorDefault);
        ShowCopyCell(toggleShowCopyCell.IsOn);
        StartCoroutine(StartRound(0));
    }

    void Update()
    {
        if (keyUpArrow.WasPressedThisFrame())
        {
            if (selectedIndex > 0)
            {
                Select(selectedIndex - 1);
            }
        }
        if (keyDownArrow.WasPressedThisFrame())
        {
            if (selectedIndex < selectables.Length - 1)
            {
                Select(selectedIndex + 1);
            }
        }
        if (keyEnter.WasPressedThisFrame())
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

        // Audio Queue
        if (audioQueuePlaying)
        {
            if (audioQueue.Count == 0)
            {
                audioQueuePlaying = false;
                brailler.SetOutputEnabled(true);
            }
            else if (delayTimer <= 0)
            {
                delayTimer = SECONDS_BETWEEN_SOUNDS_IN_QUEUE;
                PlaySound(audioQueue[0]);
                audioQueue.RemoveAt(0);
            }
            else
            {
                delayTimer -= Time.deltaTime;
            }
        }
    }
}
