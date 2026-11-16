using System.Linq;
using UnityEngine;
using System.Collections.Generic;
using Unity.VisualScripting;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    AudioSystem audioSystem;

    // Global settings
    float MasterVolume = 1f;
    float NarratorSpeed = 1f;
    bool NarratorOn = true;
    Dictionary<AudioSystem.SOUND_TYPE, float> soundSettings = new()
    {
        { AudioSystem.SOUND_TYPE.GAME, 1f },
        { AudioSystem.SOUND_TYPE.VOICE, 1f },
    };

    public float masterVolume
    {
        get { return MasterVolume; }
        set { MasterVolume = value; if (!audioSystem.IsUnityNull()) { audioSystem.OnSoundSettingsUpdate(); } }
    }
    public float narratorSpeed
    {
        get { return NarratorSpeed; }
        set { NarratorSpeed = value; if (!audioSystem.IsUnityNull()) { audioSystem.OnSoundSettingsUpdate(); } }
    }
    public bool narratorOn
    {
        get { return NarratorOn; }
        set { NarratorOn = value; if (!audioSystem.IsUnityNull()) {audioSystem.OnSoundSettingsUpdate(); }}
    }
    public void SetVolume(AudioSystem.SOUND_TYPE soundType, float value)
    {
        soundSettings[soundType] = value;
        if (!audioSystem.IsUnityNull())
        {
            audioSystem.OnSoundSettingsUpdate();
        }
    }
    public float GetVolume(AudioSystem.SOUND_TYPE soundType)
    {
        return soundSettings[soundType];
    }

    // Main menu
    public int menuIndex = 0;

    // Paper test
    public bool paperTestAudioOn = true;

    // Copy game
    public bool letterMode = true;
    public bool showCopyCell = true;

    // Built-in
    void Start()
    {
        audioSystem = AudioSystem.Instance;
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
