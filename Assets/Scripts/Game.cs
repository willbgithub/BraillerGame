using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using TMPro;
using UnityEngine.InputSystem;

public class Game : MonoBehaviour, IBrailler
{
    // Data
    int scrollToBottom = 0;
        
    InputAction keyGuide;

    // Serialized objects
    [SerializeField] Brailler brailler;
    [SerializeField] BrailleCell brailleCellPrefab;
    [SerializeField] Transform paperContent;
    [SerializeField] GameObject guide;
    [SerializeField] ScrollRect scrollView;
    [SerializeField] TMP_Text textLabel;

    void Start()
    {
        SetGuide(false);
        textLabel.text = "";
        keyGuide = InputSystem.actions.FindAction("Guide");
    }
    void Awake()
    {
        brailler.SetParent(this);
    }
    public void SetGuide(bool guide)
    {
        this.guide.SetActive(guide);
    }
    public void ToggleGuide()
    {
        SetGuide(!guide.activeSelf);
    }
    public void OnIndexWrite(int index)
    {
        CreateBrailleCell(index);
    }
    public void OnBackspaceDown()
    {
        int childCount = paperContent.childCount;
        if (childCount > 0)
        {
            BrailleCell cell = paperContent.GetChild(childCount - 1).GetComponent<BrailleCell>();
            if (cell.HasCharacter())
            {
                textLabel.text = textLabel.text.Substring(0, textLabel.text.Length - 1);
            }
            Destroy(cell.gameObject);
        }
    }
    public void OnSpaceDown()
    {
        CreateBrailleCell(0);
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
        scrollToBottom = 3;
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


    void Update()
    {
        if (keyGuide.WasPressedThisFrame())
        {
            ToggleGuide();
        }
        if (scrollToBottom >= 1)
        {
            scrollToBottom--;
            scrollView.verticalScrollbar.value = 0;
        }
    }
}
