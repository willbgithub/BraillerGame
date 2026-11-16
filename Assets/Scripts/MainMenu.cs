using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    GameManager gameManager;

    InputAction keyUpArrow;
    InputAction keyDownArrow;
    InputAction keyEnter;
    [SerializeField] GameObject image;
    [SerializeField] GameObject escMenu;

    [SerializeField] GameObject[] selectables;

    public void OnBraillerTutorialButtonPress()
    {
        SceneManager.LoadScene("Tutorial");
        Select(0);
    }
    public void OnPaperButtonPress()
    {
        SceneManager.LoadScene("PaperTest");
        Select(1);
    }
    public void OnCopyGameButtonPress()
    {
        SceneManager.LoadScene("CopyGame");
        Select(2);
    }
    public void SetEscMenu(bool value)
    {
        if (value)
        {
            Select(3);
        }
        escMenu.SetActive(value);
    }
    public void OnQuitButtonPress()
    {
        Application.Quit();
    }
    public void Select(int index)
    {
        GameObject selected = selectables[gameManager.menuIndex];
        selected.transform.GetChild(0).gameObject.SetActive(false);
        gameManager.menuIndex = index;
        selected = selectables[gameManager.menuIndex];
        selected.transform.GetChild(0).gameObject.SetActive(true);
    }
    public void OnEnterDown()
    {
        if (gameManager.menuIndex == 0)
        {
            OnBraillerTutorialButtonPress();
        }
        else if (gameManager.menuIndex == 1)
        {
            OnPaperButtonPress();
        }
        else if (gameManager.menuIndex == 2)
        {
            OnCopyGameButtonPress();
        }
        else if (gameManager.menuIndex == 3)
        {
            SetEscMenu(true);
        }
        else if (gameManager.menuIndex == 4)
        {
            OnQuitButtonPress();
        }
    }
    void Awake()
    {
        keyUpArrow = InputSystem.actions.FindAction("UpArrow");
        keyDownArrow = InputSystem.actions.FindAction("DownArrow");
        keyEnter = InputSystem.actions.FindAction("Enter");
    }
    void Start()
    {
        gameManager = GameManager.Instance;
        Select(gameManager.menuIndex);
        SetEscMenu(false);
    }
    void Update()
    {
        if (keyUpArrow.WasPressedThisFrame())
        {
            if (gameManager.menuIndex > 0)
            {
                Select(gameManager.menuIndex - 1);
            }
        }
        if (keyDownArrow.WasPressedThisFrame())
        {
            if (gameManager.menuIndex < selectables.Length - 1)
            {
                Select(gameManager.menuIndex + 1);
            }
        }
        if (keyEnter.WasPressedThisFrame())
        {
            OnEnterDown();
        }
    }
}
