using UnityEngine;

public class BrailleCell : MonoBehaviour
{
    // Serialized data

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
