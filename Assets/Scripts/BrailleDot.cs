using UnityEngine;
using UnityEngine.UI;

public class BrailleDot : MonoBehaviour
{
    // Serialized data
    [SerializeField] Image image;

    // Private member data
    BrailleCell parent;
    bool on;

    // Mutators
    public void SetParent(BrailleCell parent)
    {
        this.parent = parent;
    }
    public void SetOn(bool on)
    {
        this.on = on;
        image.gameObject.SetActive(on);
    }

    // Accessors
    public BrailleCell GetParent()
    {
        return parent;
    }
    public bool IsOn()
    {
        return on;
    }
}
