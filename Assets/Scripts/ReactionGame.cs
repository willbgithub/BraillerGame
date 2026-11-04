using TMPro;
using UnityEngine;
using System.Collections.Generic;

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
    [SerializeField] AudioClip[] audioNumberNo;
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
        PlayBrailleAudio();
    }
    public void PlayBrailleAudio()
    {
        bool[] dots = new bool[6];
        int index = copyCell.GetIndex();
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
            else
            {
                audioQueue.Add(audioNumberNo[i]);
            }
        }
        PlayAudioQueue();
    }
    public void PlayAudioQueue()
    {
        PlaySound(audioQueue[0], 1);
        delayTimer = DELAY;
        audioQueuePlaying = true;
    }
    public void PlaySound(AudioClip sound, int channel)
    {
        audioPlayer[channel].resource = sound;
        audioPlayer[channel].Play();
    }

    // Input
    public void OnIndexWrite(int index)
    {
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
            PlaySound(audioCorrect, 0);
        }
        else
        {
            feedback.text = "WRONG!";
            inputCell.SetBackgroundColor(colorWrong);
            PlaySound(audioWrong, 0);
        }
        brailler.SetOutputEnabled(false);
        StartRound(1);
    }
    public void OnBackspaceDown()
    {

    }
    public void OnSpaceDown()
    {

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
                    PlaySound(audioQueue[0], 7 - audioQueue.Count);
                }
            }
            else
            {
                delayTimer -= Time.deltaTime;
            }
        }
    }
}
