using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class Tutorial : MonoBehaviour, IBrailler
{
    [SerializeField] BrailleCell brailleCell;
    [SerializeField] Brailler brailler;
    [SerializeField] AudioClip[] audioAlphabet;
    [SerializeField] AudioClip[] audioNumbers;
    [SerializeField] AudioClip audioExclamationPoint;
    [SerializeField] AudioClip audioPeriod;
    [SerializeField] AudioClip audioQuestionMark;
    [SerializeField] AudioClip audioComma;
    [SerializeField] AudioSource audioSource;
    [SerializeField] TMP_Text characterLabel;
    [SerializeField] GameObject guide;
    InputAction keyEnter;
    InputAction keyGuide;
    public void Awake()
    {
        brailler.SetParent(this);
    }
    public void Start()
    {
        keyEnter = InputSystem.actions.FindAction("Enter");
        keyGuide = InputSystem.actions.FindAction("Guide");
        brailleCell.SetIndex(63);
        SetGuide(false);
    }
    public void SetGuide(bool guide)
    {
        this.guide.SetActive(guide);
    }
    public void ToggleGuide()
    {
        SetGuide(!guide.activeSelf);
    }
    public void OnBackButtonPress()
    {
        SceneManager.LoadScene("MainMenu");
    }
    public void Update()
    {
        if (keyEnter.WasPressedThisFrame())
        {
            OnBackButtonPress();
        }
        if (keyGuide.WasPressedThisFrame())
        {
            ToggleGuide();
        }
    }
    public void OnIndexWrite(int index)
    {
        brailleCell.SetIndex(index);
        if (brailleCell.HasCharacter())
        {
            PlayCharacterAudio(brailleCell.GetCharacter());
            characterLabel.text = brailleCell.GetCharacter().ToString();
        }
        else
        {
            characterLabel.text = "";
        }
    }
    public void OnBackspaceDown()
    {
        brailleCell.SetIndex(0);
    }
    public void OnSpaceDown()
    {
        brailleCell.SetIndex(0);
    }

    public void PlayCharacterAudio(char character)
    {
        if (character == ' ' || character == '\n')
        {
            return;
        }
        if (character >= 'a' && character <= 'z')
        {
            audioSource.resource = audioAlphabet[character - 'a'];
            audioSource.Play();
        }
        else if (character >= '0' && character <= '9')
        {
            audioSource.resource = audioNumbers[character - '0'];
        }
        else if (character == '!')
        {
            audioSource.resource = audioExclamationPoint;
        }
        else if (character == '.')
        {
            audioSource.resource = audioPeriod;
        }
        else if (character == '?')
        {
            audioSource.resource = audioQuestionMark;
        }
        else if (character == ',')
        {
            audioSource.resource = audioComma;
        }
        else
        {
            Debug.LogError($"ERROR: No audio found for '{character}'!");
            return;
        }
        audioSource.Play();
    }
}
