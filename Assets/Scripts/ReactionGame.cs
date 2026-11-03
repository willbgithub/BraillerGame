using TMPro;
using UnityEngine;
using UnityEngine.Audio;

public class ReactionGame : MonoBehaviour, IBrailler
{
    // Data
    bool prompting = false;
    float roundTimer = 0;
    bool roundTimerCounting = false;

    // Serialized Objects
    [SerializeField] Color colorDefault;
    [SerializeField] Color colorCorrect;
    [SerializeField] Color colorWrong;
    [SerializeField] Brailler brailler;
    [SerializeField] BrailleCell inputCell;
    [SerializeField] BrailleCell copyCell;
    [SerializeField] TMP_Text feedback;
    [SerializeField] AudioClip audioNewRound;
    [SerializeField] AudioClip audioCorrect;
    [SerializeField] AudioClip audioWrong;
    [SerializeField] AudioSource audioSource;

    // Game
    public void StartRound(float secDelay)
    {
        Debug.Log("StartRound(" + secDelay);
        if (secDelay > 0)
        {
            roundTimer = secDelay;
            roundTimerCounting = true;
            return;
        }
        inputCell.SetBackgroundColor(colorDefault);
        int index = Random.Range(1, 63);
        copyCell.SetIndex(index);
        PlaySound(audioNewRound);
        prompting = true;
        inputCell.SetIndex(0);
        feedback.text = "COPY!";
    }
    public void PlaySound(AudioClip sound)
    {
        audioSource.resource = sound;
        audioSource.Play();
    }

    // Input
    public void OnIndexWrite(int index)
    {
        if (prompting)
        {
            inputCell.SetIndex(index);
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
            prompting = false;
            StartRound(1);
        }
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
    }
}
