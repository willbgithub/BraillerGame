using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Key : MonoBehaviour
{
    // Custom Parameters
    [SerializeField] Color defaultColor;
    [SerializeField] Color pressedColor;
    // Serialized objects
    [SerializeField] Image background;
    [SerializeField] TMP_Text label;
    [SerializeField] AudioSource audioPlayer;

    public void OnDown()
    {
        background.color = pressedColor;
        audioPlayer.Play();
    }
    public void OnUp()
    {
        background.color = defaultColor;
    }
    public void SetLabel(string text)
    {
        label.text = text;
    }
}
