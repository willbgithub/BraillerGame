using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class ReactionGame : MonoBehaviour, IBrailler
{
    // Data
    int score = 0;
    InputAction keyGuide;

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

    // Game
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
    public void AddScore()
    {
        score++;
        labelScore.text = "Score: " + score;
    }
    IEnumerator StartRound(float secDelay = 0)
    {
        Debug.Log("StartRound " + secDelay);
        yield return new WaitForSeconds(secDelay);
        Debug.Log("StartRound bypassed");

        brailleCellInput.SetBackgroundColor(colorDefault);
        char character = (char)('a' + Random.Range(0, 23));
        while (character == brailleCellCopy.GetCharacter())
        {
            character = (char)('a' + Random.Range(0, 23));
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
        yield return new WaitForSeconds(secDelay);

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
        if (audioQueue[0] == audioNumberYes[0])
        {
            Debug.Log("1");
        }
        else if (audioQueue[0] == audioNumberYes[1])
        {
            Debug.Log("2");
        }
        else if (audioQueue[0] == audioNumberYes[2])
        {
            Debug.Log("3");
        }
        else if (audioQueue[0] == audioNumberYes[3])
        {
            Debug.Log("4");
        }
        else if (audioQueue[0] == audioNumberYes[4])
        {
            Debug.Log("5");
        }
        else if (audioQueue[0] == audioNumberYes[5])
        {
            Debug.Log("6");
        }
        PlaySound(audioQueue[0]);
        delayTimer = SECONDS_BETWEEN_SOUNDS_IN_QUEUE;
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
            AddScore();
            StartCoroutine(StartRound(1));
        }
        else
        {
            labelFeedback.text = "WRONG!";
            brailleCellInput.SetBackgroundColor(colorWrong);
            PlaySound(audioWrong);
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

    // Other
    void Awake()
    {
        brailler.SetParent(this);
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        keyGuide = InputSystem.actions.FindAction("Guide");
        SetGuide(false);
        brailleCellCopy.SetBackgroundColor(colorDefault);
        ShowCopyCell(toggleShowCopyCell.IsOn);
        StartCoroutine(StartRound(0));
    }

    // Update is called once per frame
    void Update()
    {
        // Guide
        if (keyGuide.WasPressedThisFrame())
        {
            ToggleGuide();
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
                audioQueue.RemoveAt(0);
                if (audioQueue.Count > 0)
                {
                    PlaySound(audioQueue[0]);
                    if (audioQueue[0] == audioNumberYes[0])
                    {
                        Debug.Log("1");
                    }
                    else if (audioQueue[0] == audioNumberYes[1])
                    {
                        Debug.Log("2");
                    }
                    else if (audioQueue[0] == audioNumberYes[2])
                    {
                        Debug.Log("3");
                    }
                    else if (audioQueue[0] == audioNumberYes[3])
                    {
                        Debug.Log("4");
                    }
                    else if (audioQueue[0] == audioNumberYes[4])
                    {
                        Debug.Log("5");
                    }
                    else if (audioQueue[0] == audioNumberYes[5])
                    {
                        Debug.Log("6");
                    }
                }
            }
            else
            {
                delayTimer -= Time.deltaTime;
            }
        }
    }
}
