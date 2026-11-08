using Unity.VisualScripting;
using UnityEngine;

public class Toggle : MonoBehaviour
{
    IToggle parent;
    [SerializeField] bool isOn;
    [SerializeField] GameObject checkmark;
    public void SetParent(IToggle parent)
    {
        this.parent = parent;
    }
    private void OnValidate()
    {
        checkmark.SetActive(isOn);
        if (parent.IsUnityNull())
            return;
        parent.OnValueChanged(this, isOn);
    }
    public void OnPointerClick()
    {
        isOn = !isOn;
        parent.OnValueChanged(this, isOn);
        checkmark.SetActive(isOn);
    }
}
