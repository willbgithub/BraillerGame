using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class Toggle : MonoBehaviour
{
    [SerializeField] bool isOn;
    public bool IsOn
    {
        get => isOn;
        set
        { 
            isOn = value;
            checkmark.SetActive(value);
            OnValueChanged.Invoke(value);
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
}
