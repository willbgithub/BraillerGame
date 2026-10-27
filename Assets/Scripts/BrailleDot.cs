using UnityEngine;
using UnityEngine.UI;

public class BrailleDot : MonoBehaviour
{
    // Serialized data
    [SerializeField] Image image;

    // Private member data
    const float OFF_SCALE = 0.15f;
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
        if (on)
        {
            image.transform.localScale = new Vector3(1, 1, 1);
        }
        else
        {
            image.transform.localScale = new Vector3(OFF_SCALE, OFF_SCALE, OFF_SCALE);
        }
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
