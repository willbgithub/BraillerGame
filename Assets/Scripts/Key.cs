using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Key : MonoBehaviour
{
    // Serialized objects
    [SerializeField] Color defaultColor;
    [SerializeField] Color pressedColor;
    [SerializeField] Image background;
    [SerializeField] TMP_Text label;

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
    public void SetLabel(string text)
    {
        label.text = text;
    }
}
