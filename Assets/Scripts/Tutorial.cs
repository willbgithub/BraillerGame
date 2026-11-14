using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class Tutorial : MonoBehaviour
{
    [SerializeField] BrailleCell brailleCell;
    InputAction keyEnter;
    public void Start()
    {
        keyEnter = InputSystem.actions.FindAction("Enter");
        brailleCell.SetIndex(63);
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
    }
}
