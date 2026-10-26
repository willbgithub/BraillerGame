using UnityEngine;

public class BrailleCell : MonoBehaviour
{
    // Serialized data

    // Private member data
    BrailleDot[] dots;
    char character;

    // Mutators
    public void SetCharacter(char character)
    {
        //Debug.Log("cell: SetCharacter(" + character + ")");
        character = char.ToUpper(character);
        if (!Braille.alphabet.ContainsKey(character))
        {
            Debug.LogError("ERROR: " + character + " does not have a braille index!");
            return;
        }
        this.character = character;
        SetIndex(Braille.alphabet[character]);
    }
    void SetIndex(int index)
    {
        Debug.Log("cell: SetIndex(" + index + ")");
        for (int i = 5; i >= 0; i--)
        {
            Debug.Log("i: " + i);
            int value = (int)Mathf.Pow(2, i);
            Debug.Log("value: " + value);
            if (index >= value)
            {
                Debug.Log("index (" + index + ") >= value (" + value + ")");
                index -= value;
                Debug.Log("new index: " + index);
                dots[i].SetOn(true);
                Debug.Log("dot enabled");
            }
            else
            {
                Debug.Log("index (" + index + ") < value (" + value + ")");
                dots[i].SetOn(false);
                Debug.Log("dot disabled");
            }
        }
    }
    // Accessors
    public char GetCharacter()
    {
        return character;
    }
    public int GetIndex()
    {
        return Braille.alphabet[character];
    }
    // Other
    void Start()
    {
        dots = new BrailleDot[6];
        for (int i = 0; i < 6; i++)
        {
            dots[i] = transform.GetChild(i).GetComponent<BrailleDot>();
            dots[i].SetParent(this);
            dots[i].SetOn(false);
        }
    }
}
