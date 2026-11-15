using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    InputAction keyUpArrow;
    InputAction keyDownArrow;
    InputAction keyEnter;

    [SerializeField] GameObject[] selectables;
    int selectedIndex;

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
        SceneManager.LoadScene("ReactionGame");
    }
    public void OnQuitButtonPress()
    {
        Application.Quit();
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
            OnBraillerTutorialButtonPress();
        }
        else if (selectedIndex == 1)
        {
            OnPaperButtonPress();
        }
        else if (selectedIndex == 2)
        {
            OnCopyGameButtonPress();
        }
        else if (selectedIndex == 3)
        {
            OnQuitButtonPress();
        }
    }
    void Start()
    {
        keyUpArrow = InputSystem.actions.FindAction("UpArrow");
        keyDownArrow = InputSystem.actions.FindAction("DownArrow");
        keyEnter = InputSystem.actions.FindAction("Enter");

        Select(0);
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
                Select(selectedIndex+1);
            }
        }
        if (keyEnter.WasPressedThisFrame())
        {
            OnEnterDown();
        }
    }
}
