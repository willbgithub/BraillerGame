using UnityEngine;

public class Game : MonoBehaviour
{
    // Serialized objects
    [SerializeField] BrailleCell cell;

    void Update()
    {
        if (UnityEngine.InputSystem.Keyboard.current.aKey.wasPressedThisFrame)
        {
            //Debug.Log("A pressed");
            cell.SetCharacter('a');
        }
        else if (UnityEngine.InputSystem.Keyboard.current.bKey.wasPressedThisFrame)
        {
            //Debug.Log("B pressed");
            cell.SetCharacter('b');
        }
        else if (UnityEngine.InputSystem.Keyboard.current.cKey.wasPressedThisFrame)
        {
            //Debug.Log("C pressed");
            cell.SetCharacter('c');
        }
    }
}
