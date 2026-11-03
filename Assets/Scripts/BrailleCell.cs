using UnityEngine;
using UnityEngine.UI;

#pragma warning disable CS0660 // Type defines operator == or operator != but does not override Object.Equals(object o)
#pragma warning disable CS0661 // Type defines operator == or operator != but does not override Object.GetHashCode()
public class BrailleCell : MonoBehaviour
#pragma warning restore CS0661 // Type defines operator == or operator != but does not override Object.GetHashCode()
#pragma warning restore CS0660 // Type defines operator == or operator != but does not override Object.Equals(object o)
{
    // Serialized data
    [SerializeField] Image background;

    // Private member data
    BrailleDot[] dots;
    int index;
    bool started = false;

    // Mutators
    public void SetCharacter(char character)
    {
        //Debug.Log("cell: SetCharacter(" + character + ")");
        character = char.ToUpper(character);
        if (!Braille.ContainsCharacter(character))
        {
            Debug.LogError("ERROR: \"" + character + "\" cannot be represented in Braille!");
            return;
        }
        SetIndex(Braille.GetIndex(character));
    }
    public void SetIndex(int index)
    {
        this.index = index;
        // Update dots
        if (!started)
        {
            return;
        }
        for (int i = 5; i >= 0; i--)
        {
            int value = (int)Mathf.Pow(2, i);
            if (index >= value)
            {
                index -= value;
                dots[i].SetOn(true);
            }
            else
            {
                dots[i].SetOn(false);
            }
        }
    }
    public void SetBackgroundColor(Color color)
    {
        background.color = color;
    }
    // Accessors
    public char GetCharacter()
    {
        return Braille.GetCharacter(index);
    }
    public int GetIndex()
    {
        return index;
    }
    public bool HasCharacter()
    {
        return Braille.ContainsIndex(index);
    }
    public static bool operator ==(BrailleCell cell1, BrailleCell cell2)
    {
        return cell1.index == cell2.index;
    }
    public static bool operator !=(BrailleCell cell1, BrailleCell cell2)
    {
        return cell1.index != cell2.index;
    }
    // Other
    void Start()
    {
        started = true;
        dots = new BrailleDot[6];
        for (int i = 0; i < 6; i++)
        {
            dots[i] = transform.GetChild(i).GetComponent<BrailleDot>();
            dots[i].SetParent(this);
            dots[i].SetOn(false);
        }
        SetIndex(index);
    }
}
