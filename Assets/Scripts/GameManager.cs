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
        { AudioSystem.SOUND_TYPE.IMMUNE, 1f },
        { AudioSystem.SOUND_TYPE.GAME, 1f },
        { AudioSystem.SOUND_TYPE.VOICE, 1f },
        { AudioSystem.SOUND_TYPE.NARRATOR, 1f },
    };

    public float masterVolume
    {
        get { return MasterVolume; }
        set { MasterVolume = value; PlayerPrefs.SetFloat("masterVolume", value); if (!audioSystem.IsUnityNull()) { audioSystem.OnSoundSettingsUpdate(); } }
    }
    public float narratorSpeed
    {
        get { return NarratorSpeed; }
        set { NarratorSpeed = value; PlayerPrefs.SetFloat("narratorSpeed", value); if (!audioSystem.IsUnityNull()) { audioSystem.OnSoundSettingsUpdate(); } }
    }
    public bool narratorOn
    {
        get { return NarratorOn; }
        set { NarratorOn = value; PlayerPrefs.SetInt("narratorOn", value?1:0); if (!audioSystem.IsUnityNull()) {audioSystem.OnSoundSettingsUpdate(); }}
    }
    public void SetVolume(AudioSystem.SOUND_TYPE soundType, float value)
    {
        if (soundType == AudioSystem.SOUND_TYPE.VOICE || soundType == AudioSystem.SOUND_TYPE.NARRATOR)
        {
            soundSettings[AudioSystem.SOUND_TYPE.VOICE] = value;
            soundSettings[AudioSystem.SOUND_TYPE.NARRATOR] = value;
        }
        else
        {
            soundSettings[soundType] = value;
        }
        if (!audioSystem.IsUnityNull())
        {
            audioSystem.OnSoundSettingsUpdate();
        }
        PlayerPrefs.SetFloat($"{soundType}", value);
    }
    public float GetVolume(AudioSystem.SOUND_TYPE soundType)
    {
        return soundSettings[soundType];
    }

    // Main menu
    public int menuIndex = 0;

    // Paper test
    bool paperTestAudioOn = true;
    public bool PaperTestAudioOn
    {
        get { return paperTestAudioOn; }
        set { paperTestAudioOn = value; PlayerPrefs.SetInt("paperTestAudioOn", value ? 1 : 0); }
    }
    bool paperTestTutorialSeen = false;
    public bool PaperTestTutorialSeen
    {
        get { return paperTestTutorialSeen; }
        set { paperTestTutorialSeen = value; PlayerPrefs.SetInt("paperTestTutorialSeen", value ? 1 : 0); }
    }

    // Copy game
    bool letterMode = true;
    public bool LetterMode
    {
        get { return letterMode; }
        set { letterMode = value; PlayerPrefs.SetInt("letterMode", value ? 1 : 0); }
    }
    bool showCopyCell = true;
    public bool ShowCopyCell
    {
        get { return showCopyCell; }
        set { showCopyCell = value; PlayerPrefs.SetInt("showCopyCell", value ? 1 : 0); }
    }
    bool copyGameTutorialSeen = false;
    public bool CopyGameTutorialSeen
    {
        get { return copyGameTutorialSeen; }
        set
        {
            copyGameTutorialSeen = value;
            PlayerPrefs.SetInt("copyGameTutorialSeen", value ? 1 : 0);
        }
    }

    // Built-in
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
    void Start()
    {
        audioSystem = AudioSystem.Instance;

        masterVolume = PlayerPrefs.GetFloat("masterVolume", 1f);
        SetVolume(AudioSystem.SOUND_TYPE.VOICE, PlayerPrefs.GetFloat("VOICE", 1f));
        SetVolume(AudioSystem.SOUND_TYPE.GAME, PlayerPrefs.GetFloat("GAME", 1f));
        narratorSpeed = PlayerPrefs.GetFloat("narratorSpeed", 1f);
        narratorOn = PlayerPrefs.GetInt("narratorOn", 1) != 0;

        paperTestAudioOn = PlayerPrefs.GetInt("paperTestAudioOn", 1) != 0;
        copyGameTutorialSeen = PlayerPrefs.GetInt("copyGameTutorialSeen", 0) != 0;
        paperTestTutorialSeen = PlayerPrefs.GetInt("paperTestTutorialSeen", 0) != 0;
        letterMode = PlayerPrefs.GetInt("letterMode", 1) != 0;
        showCopyCell = PlayerPrefs.GetInt("showCopyCell", 1) != 0;
    }
    void Update()
    {

    }
}
