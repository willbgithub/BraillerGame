using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Game : MonoBehaviour, IBrailler
{
    GameManager gameManager;
    public AudioSystem audioSystem;

    public void OnBackButtonPress()
    {
        SceneManager.LoadScene("MainMenu");
    }

    // Data
    [SerializeField] GameObject[] selectables;
    int selectedIndex;

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
    [SerializeField] GameObject escMenu;
    [SerializeField] ScrollRect scrollView;
    [SerializeField] ScrollRect textScrollView;
    [SerializeField] TMP_Text textLabel;
    [SerializeField] Toggle audioToggle;

    [SerializeField] AudioClip testSound;

    public void SetAudioEnabled(bool value)
    {
        gameManager.paperTestAudio = value;
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
            OnClearButtonPress();
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
    }
    void Start()
    {
        gameManager = GameManager.Instance;
        audioSystem = AudioSystem.Instance;
        audioToggle.IsOn = gameManager.paperTestAudio;
        Select(0);
        SetGuide(false);
        SetEscMenu(false);
    }
    void Update()
    {
        if (keyGuide.WasPressedThisFrame())
        {
            ToggleGuide();
        }
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
