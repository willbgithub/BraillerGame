using TMPro;
using UnityEngine;

public class ReactionGame : MonoBehaviour, IBrailler
{
    // Data

    // Serialized Objects
    [SerializeField] Brailler brailler;
    [SerializeField] BrailleCell inputCell;
    [SerializeField] BrailleCell copyCell;
    [SerializeField] TMP_Text feedback;

    // Game


    // Input
    public void OnIndexWrite(int index)
    {

    }
    public void OnBackspaceDown()
    {

    }
    public void OnSpaceDown()
    {

    }

    // Other
    void Awake()
    {
        brailler.SetParent(this);
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
