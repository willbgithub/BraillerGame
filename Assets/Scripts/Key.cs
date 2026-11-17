using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Key : MonoBehaviour
{
    AudioSystem audioSystem;

    // Custom Parameters
    [SerializeField] Color defaultColor;
    [SerializeField] Color pressedColor;
    // Serialized objects
    [SerializeField] Image background;
    [SerializeField] TMP_Text label;
    [SerializeField] AudioClip audioKeyPress;
    [SerializeField] AudioClip audioKeyPressFail;

    public void OnDown(bool outputEnabled)
    {
        background.color = pressedColor;
        if (outputEnabled)
        {
            audioSystem.PlaySound(audioKeyPress, AudioSystem.SOUND_TYPE.GAME);
        }
        else
        {
            audioSystem.PlaySound(audioKeyPressFail, AudioSystem.SOUND_TYPE.GAME);
        }
    }
    public void OnUp()
    {
        background.color = defaultColor;
    }
    public void SetLabel(string text)
    {
        label.text = text;
    }

    void Awake()
    {

    }
    void Start()
    {
        audioSystem = AudioSystem.Instance;
    }
    void Update()
    {
        
    }
}
