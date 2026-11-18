using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class Toggle : MonoBehaviour
{
    [SerializeField] bool isOn;
    [SerializeField] AudioClip narrationEnable;
    [SerializeField] AudioClip narrationDisable;
    [SerializeField] AudioClip soundEnable;
    [SerializeField] AudioClip soundDisable;

    GameManager gameManager;
    AudioSystem audioSystem;

    public bool IsOn
    {
        get => isOn;
        set
        { 
            isOn = value;
            checkmark.SetActive(value);
            OnValueChanged.Invoke(value);
            if (gameManager.narratorOn)
            {
                if (value)
                {
                    audioSystem.PlaySound(narrationEnable, AudioSystem.SOUND_TYPE.NARRATOR);
                }
                else
                {
                    audioSystem.PlaySound(narrationDisable, AudioSystem.SOUND_TYPE.NARRATOR);
                }
            }
            else
            {
                if (value)
                {
                    audioSystem.PlaySound(soundEnable, AudioSystem.SOUND_TYPE.NARRATOR);
                }
                else
                {
                    audioSystem.PlaySound(soundDisable, AudioSystem.SOUND_TYPE.NARRATOR);
                }
            }
        }
    }
    [SerializeField] GameObject checkmark;
    [SerializeField] UnityEvent<bool> OnValueChanged;
    private void OnValidate()
    {
        checkmark.SetActive(isOn);
    }
    public void OnPointerClick()
    {
        IsOn = !isOn;
    }
    public void ToggleValue()
    {
        IsOn = !isOn;
    }

    void Awake()
    {

    }
    void Start()
    {
        gameManager = GameManager.Instance;
        audioSystem = AudioSystem.Instance;
    }
}
