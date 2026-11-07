using TMPro;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class ReactionGame : MonoBehaviour, IBrailler
{
    // Data
    float roundTimer = 0;
    bool roundTimerCounting = false;

    List<AudioClip> audioQueue = new List<AudioClip>();
    bool audioQueuePlaying = false;
    const float DELAY = 0.2f;
    float delayTimer = 0;

    // Serialized Objects
    [SerializeField] Color colorDefault;
    [SerializeField] Color colorCorrect;
    [SerializeField] Color colorWrong;
    [SerializeField] Brailler brailler;
    [SerializeField] BrailleCell inputCell;
    [SerializeField] BrailleCell copyCell;
    [SerializeField] TMP_Text feedback;
    [SerializeField] TMP_Text inputLetterLabel;
    [SerializeField] TMP_Text copyLetterLabel;
    [SerializeField] AudioClip audioNewRound;
    [SerializeField] AudioClip audioCorrect;
    [SerializeField] AudioClip audioWrong;
    [SerializeField] AudioClip[] audioNumberYes;
    [SerializeField] AudioSource[] audioPlayer;

    // Game
    public void StartRound(float secDelay)
    {
        if (secDelay > 0)
        {
            roundTimer = secDelay;
            roundTimerCounting = true;
            return;
        }
        inputCell.SetBackgroundColor(colorDefault);
        int index = Random.Range(0, 23);
        char character = (char)('a' + index);
        copyCell.SetCharacter(character);
        copyLetterLabel.text = character.ToString();
        inputCell.SetIndex(0);
        inputLetterLabel.text = "";
        feedback.text = "COPY!";
        PlayBrailleAudio(copyCell.GetIndex());
    }
    public void PlayBrailleAudio(int index)
    {
        brailler.SetOutputEnabled(false);
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
        delayTimer = DELAY;
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
        inputCell.SetIndex(index);
        if (inputCell.HasCharacter())
        {
            char character = inputCell.GetCharacter();
            inputLetterLabel.text = character.ToString();
        }
        else
        {
            inputLetterLabel.text = "Not a character!";
        }
        if (index == copyCell.GetIndex())
        {
            feedback.text = "CORRECT!";
            inputCell.SetBackgroundColor(colorCorrect);
            PlaySound(audioCorrect);
        }
        else
        {
            feedback.text = "WRONG!";
            inputCell.SetBackgroundColor(colorWrong);
            PlaySound(audioWrong);
        }
        StartRound(1);
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
        PlayBrailleAudio(copyCell.GetIndex());
    }

    // Other
    void Awake()
    {
        brailler.SetParent(this);
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        copyCell.SetBackgroundColor(colorDefault);
        StartRound(0);
    }

    // Update is called once per frame
    void Update()
    {
        if (roundTimer > 0)
        {
            roundTimer -= Time.deltaTime;
        }
        else if (roundTimerCounting && roundTimer <= 0)
        {
            roundTimerCounting = false;
            StartRound(0);
        }

        if (audioQueuePlaying)
        {
            if (audioQueue.Count == 0)
            {
                audioQueuePlaying = false;
                brailler.SetOutputEnabled(true);
            }
            else if (delayTimer <= 0)
            {
                delayTimer = DELAY;
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
