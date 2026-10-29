using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.InputSystem;

public class Brailler : MonoBehaviour
{
    // Data
    IBrailler parent;
    [SerializeField] float submitDelay = 0.025f;
    float timer;
    bool counting;
    Dictionary<Key, bool> pressedKeys;

    InputAction key0Input;
    InputAction key1Input;
    InputAction key2Input;
    InputAction key3Input;
    InputAction key4Input;
    InputAction key5Input;
    InputAction keyBackspaceInput;
    InputAction keySpaceInput;

    // Serialized Objects
    [SerializeField] Key[] keys;
    [SerializeField] Key keySpace;
    [SerializeField] Key keyBackspace;

    public void SetParent(IBrailler parent)
    {
        this.parent = parent;
    }
    public void OnTimerExpire()
    {
        int index = 0;
        for (int i = 0; i < 6; i++)
        {
            if (pressedKeys[keys[i]])
            {
                index += (int)Mathf.Pow(2, i);
                pressedKeys[keys[i]] = false;
            }
        }
        OnIndexWrite(index);
    }
    public void OnIndexWrite(int index)
    {
        parent.OnIndexWrite(index);
    }
    public void OnBackspaceDown()
    {
        keyBackspace.OnDown();
        parent.OnBackspaceDown();
    }
    public void OnBackspaceUp()
    {
        keyBackspace.OnUp();
    }
    public void OnSpaceDown()
    {
        keySpace.OnDown();
        parent.OnSpaceDown();
    }
    public void OnSpaceUp()
    {
        keySpace.OnUp();
    }
    void Start()
    {
        pressedKeys = new()
        {
            {keys[0], false},
            {keys[1], false},
            {keys[2], false},
            {keys[3], false},
            {keys[4], false},
            {keys[5], false},
        };
        counting = false;
        timer = 0;
        keys[0].SetLabel(InputToString(key0Input));
        keys[1].SetLabel(InputToString(key1Input));
        keys[2].SetLabel(InputToString(key2Input));
        keys[3].SetLabel(InputToString(key3Input));
        keys[4].SetLabel(InputToString(key4Input));
        keys[5].SetLabel(InputToString(key5Input));
        keySpace.SetLabel(InputToString(keySpaceInput));
        keyBackspace.SetLabel(InputToString(keyBackspaceInput));
    }
    static string InputToString(InputAction action)
    {
        string match = Regex.Match(action.bindings[0].path, "/(.+)").Groups[1].Value;
        if (match == "semicolon")
        {
            return ";";
        }
        char character = char.ToUpper(match[0]);
        return character + match.Substring(1);
    }
    void Awake()
    {
        key0Input = InputSystem.actions.FindAction("Key0");
        key1Input = InputSystem.actions.FindAction("Key1");
        key2Input = InputSystem.actions.FindAction("Key2");
        key3Input = InputSystem.actions.FindAction("Key3");
        key4Input = InputSystem.actions.FindAction("Key4");
        key5Input = InputSystem.actions.FindAction("Key5");
        keySpaceInput = InputSystem.actions.FindAction("KeySpace");
        keyBackspaceInput = InputSystem.actions.FindAction("KeyBackspace");
    }
    void Update()
    {
        bool keyPressed = false;
        if (key0Input.WasPressedThisFrame())
        {
            keys[0].OnDown();
            pressedKeys[keys[0]] = true;
            keyPressed = true;
        }
        else if (key0Input.WasReleasedThisFrame())
        {
            keys[0].OnUp();
        }
        if (key1Input.WasPressedThisFrame())
        {
            keys[1].OnDown();
            pressedKeys[keys[1]] = true;
            keyPressed = true;
        }
        else if (key1Input.WasReleasedThisFrame())
        {
            keys[1].OnUp();
        }
        if (key2Input.WasPressedThisFrame())
        {
            keys[2].OnDown();
            pressedKeys[keys[2]] = true;
            keyPressed = true;
        }
        else if (key2Input.WasReleasedThisFrame())
        {
            keys[2].OnUp();
        }
        if (keySpaceInput.WasPressedThisFrame())
        {
            keySpace.OnDown();
            counting = false;
            timer = 0;
            OnSpaceDown();
        }
        else if (keySpaceInput.WasReleasedThisFrame())
        {
            OnSpaceUp();
        }
        if (key3Input.WasPressedThisFrame())
        {
            keys[3].OnDown();
            pressedKeys[keys[3]] = true;
            keyPressed = true;
        }
        else if (key3Input.WasReleasedThisFrame())
        {
            keys[3].OnUp();
        }
        if (key4Input.WasPressedThisFrame())
        {
            keys[4].OnDown();
            pressedKeys[keys[4]] = true;
            keyPressed = true;
        }
        else if (key4Input.WasReleasedThisFrame())
        {
            keys[4].OnUp();
        }
        if (key5Input.WasPressedThisFrame())
        {
            keys[5].OnDown();
            pressedKeys[keys[5]] = true;
            keyPressed = true;
        }
        else if (key5Input.WasReleasedThisFrame())
        {
            keys[5].OnUp();
        }
        if (keyBackspaceInput.WasPressedThisFrame())
        {
            OnBackspaceDown();
        }
        else if (keyBackspaceInput.WasReleasedThisFrame())
        {
            OnBackspaceUp();
        }
        if (keyPressed && timer <= 0)
        {
            timer = submitDelay;
            counting = true;
        }
        else if (counting && timer <= 0)
        {
            counting = false;
            OnTimerExpire();
        }
        else if (counting)
        {
            timer -= Time.deltaTime;
        }
    }
}
