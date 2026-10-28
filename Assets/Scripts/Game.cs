using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using TMPro;

public class Game : MonoBehaviour
{
    // Data
    [SerializeField] float submitDelay = 0.025f;
    float timer;
    bool counting;
    Dictionary<Key, bool> pressedKeys;

    // Serialized objects
    [SerializeField] BrailleCell brailleCellPrefab;
    [SerializeField] Transform paperContent;
    [SerializeField] ScrollRect scrollView;
    [SerializeField] TMP_Text textLabel;
    [SerializeField] Key[] keys;
    [SerializeField] Key keySpace;

    void Start()
    {
        textLabel.text = "";
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
    }
    void Update()
    {
        bool keyPressed = false;
        if (UnityEngine.InputSystem.Keyboard.current.sKey.wasPressedThisFrame)
        {
            keys[2].OnDown();
            pressedKeys[keys[2]] = true;
            keyPressed = true;
        }
        else if (UnityEngine.InputSystem.Keyboard.current.sKey.wasReleasedThisFrame)
        {
            keys[2].OnUp();
        }
        if (UnityEngine.InputSystem.Keyboard.current.dKey.wasPressedThisFrame)
        {
            keys[1].OnDown();
            pressedKeys[keys[1]] = true;
            keyPressed = true;
        }
        else if (UnityEngine.InputSystem.Keyboard.current.dKey.wasReleasedThisFrame)
        {
            keys[1].OnUp();
        }
        if (UnityEngine.InputSystem.Keyboard.current.fKey.wasPressedThisFrame)
        {
            keys[0].OnDown();
            pressedKeys[keys[0]] = true;
            keyPressed = true;
        }
        else if (UnityEngine.InputSystem.Keyboard.current.fKey.wasReleasedThisFrame)
        {
            keys[0].OnUp();
        }
        if (UnityEngine.InputSystem.Keyboard.current.spaceKey.wasPressedThisFrame)
        {
            keySpace.OnDown();
            counting = false;
            timer = 0;
            CreateBrailleCell(0);
        }
        else if (UnityEngine.InputSystem.Keyboard.current.spaceKey.wasReleasedThisFrame)
        {
            keySpace.OnUp();
        }
        if (UnityEngine.InputSystem.Keyboard.current.jKey.wasPressedThisFrame)
        {
            keys[3].OnDown();
            pressedKeys[keys[3]] = true;
            keyPressed = true;
        }
        else if (UnityEngine.InputSystem.Keyboard.current.jKey.wasReleasedThisFrame)
        {
            keys[3].OnUp();
        }
        if (UnityEngine.InputSystem.Keyboard.current.kKey.wasPressedThisFrame)
        {
            keys[4].OnDown();
            pressedKeys[keys[4]] = true;
            keyPressed = true;
        }
        else if (UnityEngine.InputSystem.Keyboard.current.kKey.wasReleasedThisFrame)
        {
            keys[4].OnUp();
        }
        if (UnityEngine.InputSystem.Keyboard.current.lKey.wasPressedThisFrame)
        {
            keys[5].OnDown();
            pressedKeys[keys[5]] = true;
            keyPressed = true;
        }
        else if (UnityEngine.InputSystem.Keyboard.current.lKey.wasReleasedThisFrame)
        {
            keys[5].OnUp();
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
        CreateBrailleCell(index);
    }
    // Index of 0 is a space
    public void CreateBrailleCell(int index)
    {
        BrailleCell brailleCell = Instantiate(brailleCellPrefab, paperContent);
        brailleCell.SetIndex(index);
        if (brailleCell.HasCharacter())
        {
            textLabel.text += brailleCell.GetCharacter();
        }
        scrollView.verticalScrollbar.value = 0;
    }
    public void OnClearButtonPress()
    {
        textLabel.text = "";
        int childCount = paperContent.childCount;
        for (int i = 0; i < childCount; i++)
        {
            Destroy(paperContent.GetChild(i).gameObject);
        }
    }
}
