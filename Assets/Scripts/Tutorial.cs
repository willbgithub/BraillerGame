using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class Tutorial : MonoBehaviour, IBrailler
{
    GameManager gameManager;
    AudioSystem audioSystem;

    [SerializeField] BrailleCell brailleCell;
    [SerializeField] Brailler brailler;
    [SerializeField] TMP_Text characterLabel;
    [SerializeField] GameObject guide;
    [SerializeField] GameObject escMenu;
    InputAction keyEnter;
    InputAction keyGuide;
    InputAction keyEsc;
    public void SetEscMenu(bool escMenu)
    {
        this.escMenu.SetActive(escMenu);
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
    public void OnBackButtonPress()
    {
        SceneManager.LoadScene("MainMenu");
    }
    public void OnIndexWrite(int index)
    {
        brailleCell.SetIndex(index);
        if (brailleCell.HasCharacter())
        {
            audioSystem.PlayCharacterAudio(brailleCell.GetCharacter());
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

    
    public void Awake()
    {
        brailler.SetParent(this);
        keyEnter = InputSystem.actions.FindAction("Enter");
        keyGuide = InputSystem.actions.FindAction("Guide");
        keyEsc = InputSystem.actions.FindAction("Esc");
    }
    public void Start()
    {
        gameManager = GameManager.Instance;
        audioSystem = AudioSystem.Instance;

        brailleCell.SetIndex(63);
        SetGuide(false);
        SetEscMenu(false);
    }
    public void Update()
    {
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
