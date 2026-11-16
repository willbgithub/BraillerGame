using System.Linq;
using UnityEngine;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    // Global settings
    public float masterVolume = 1f;
    public Dictionary<AudioSystem.SOUND_TYPE, float> soundSettings = new()
    {
        { AudioSystem.SOUND_TYPE.GAME, 1f },
        { AudioSystem.SOUND_TYPE.VOICE, 1f },
    };
    public float narratorSpeed = 1f;
    public bool narratorOn = true;

    // Main menu
    public int selectedIndex = 0;

    // Paper test
    public bool paperTestAudio = true;

    // Copy game
    public bool letterMode = true;
    public bool showCopyCell = true;

    void Start()
    {

    }
    void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }
    void Update()
    {

    }
}
