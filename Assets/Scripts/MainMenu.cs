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

    [SerializeField] GameObject[] selectables;

    public void OnBraillerTutorialButtonPress()
    {
        SceneManager.LoadScene("Tutorial");
    }
    public void OnPaperButtonPress()
    {
        SceneManager.LoadScene("PaperTest");
    }
    public void OnCopyGameButtonPress()
    {
        SceneManager.LoadScene("CopyGame");
    }
    public void OnQuitButtonPress()
    {
        Application.Quit();
    }
    public void Select(int index)
    {
        GameObject selected = selectables[gameManager.selectedIndex];
        selected.transform.GetChild(0).gameObject.SetActive(false);
        gameManager.selectedIndex = index;
        selected = selectables[gameManager.selectedIndex];
        selected.transform.GetChild(0).gameObject.SetActive(true);
    }
    public void OnEnterDown()
    {
        if (gameManager.selectedIndex == 0)
        {
            OnBraillerTutorialButtonPress();
        }
        else if (gameManager.selectedIndex == 1)
        {
            OnPaperButtonPress();
        }
        else if (gameManager.selectedIndex == 2)
        {
            OnCopyGameButtonPress();
        }
        else if (gameManager.selectedIndex == 3)
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
        Select(gameManager.selectedIndex);
    }
    void Update()
    {
        if (keyUpArrow.WasPressedThisFrame())
        {
            if (gameManager.selectedIndex > 0)
            {
                Select(gameManager.selectedIndex - 1);
            }
        }
        if (keyDownArrow.WasPressedThisFrame())
        {
            if (gameManager.selectedIndex < selectables.Length - 1)
            {
                Select(gameManager.selectedIndex + 1);
            }
        }
        if (keyEnter.WasPressedThisFrame())
        {
            OnEnterDown();
        }
    }
}
