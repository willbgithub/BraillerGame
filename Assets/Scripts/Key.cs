using UnityEngine;
using UnityEngine.UI;

public class Key : MonoBehaviour
{
    // Serialized objects
    [SerializeField] Color defaultColor;
    [SerializeField] Color pressedColor;
    [SerializeField] Image background;

    // Private member data
    //bool pressed;

    public void OnDown()
    {
        //pressed = true;
        background.color = pressedColor;
    }
    public void OnUp()
    {
        //pressed = false;
        background.color = defaultColor;
    }
}
