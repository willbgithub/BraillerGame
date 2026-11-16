using System.Collections.Generic;
using System.Linq;
using Unity.Collections;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.Audio;
using static AudioSystem;

public class AudioSystem : MonoBehaviour
{
    // Data
    public static AudioSystem Instance;
    GameManager gameManager;
    Dictionary<AudioSource, SOUND_TYPE> audioSources;
    AudioMixerGroup gameMixer;
    AudioMixerGroup voiceMixer;
    public enum SOUND_TYPE { GAME, VOICE };

    int maxChannels = 10;
    Dictionary<char, AudioClip> audioDictionary;

    // Audio Queue
    List<AudioClip> audioQueue = new List<AudioClip>();
    bool audioQueuePlaying = false;
    float secondsBetweenSounds = 0;
    float delayTimer = 0;

    // Functions
    public float PlayCharacterQueue(List<char> queue)
    {
        audioQueue = new List<AudioClip>();
        int queueCount = queue.Count;
        for (int i = 0; i < queueCount; i++)
        {
            audioQueue.Add(audioDictionary[queue[i]]);
        }
        secondsBetweenSounds = 0.8f / gameManager.narratorSpeed;
        delayTimer = 0;
        audioQueuePlaying = true;
        return secondsBetweenSounds * queueCount;
    }
    public void Stop()
    {
        for (int i = 0; i < audioSources.Count; i++)
        {
            audioSources.ElementAt(i).Key.Stop();
        }
    }
    public void OnSoundSettingsUpdate()
    {
        float inverse = Mathf.Sqrt(1 / gameManager.narratorSpeed);
        voiceMixer.audioMixer.SetFloat("pitchBend", inverse);
        voiceMixer.audioMixer.SetFloat("pitchBend2", inverse);
        for (int i = 0; i < audioSources.Count; i++)
        {
            AudioSource audioSource = audioSources.ElementAt(i).Key;
            SOUND_TYPE soundType = audioSources.ElementAt(i).Value;
            // Volume
            audioSource.volume = gameManager.masterVolume * gameManager.GetVolume(soundType);
            // Pitch
            if (soundType == SOUND_TYPE.VOICE)
            {
                Debug.Log('v');
                audioSource.pitch = gameManager.narratorSpeed;
            }
        }
    }
    public void CreateDict()
    {
        audioDictionary = new Dictionary<char, AudioClip>()
        {
            // Alphabet
            {'a', AssetDatabase.LoadAssetAtPath<AudioClip>("Assets/Sounds/alphabet/a.mp3")},
            {'b', AssetDatabase.LoadAssetAtPath<AudioClip>("Assets/Sounds/alphabet/b.mp3")},
            {'c', AssetDatabase.LoadAssetAtPath<AudioClip>("Assets/Sounds/alphabet/c.mp3")},
            {'d', AssetDatabase.LoadAssetAtPath<AudioClip>("Assets/Sounds/alphabet/d.mp3")},
            {'e', AssetDatabase.LoadAssetAtPath<AudioClip>("Assets/Sounds/alphabet/e.mp3")},
            {'f', AssetDatabase.LoadAssetAtPath<AudioClip>("Assets/Sounds/alphabet/f.mp3")},
            {'g', AssetDatabase.LoadAssetAtPath<AudioClip>("Assets/Sounds/alphabet/g.mp3")},
            {'h', AssetDatabase.LoadAssetAtPath<AudioClip>("Assets/Sounds/alphabet/h.mp3")},
            {'i', AssetDatabase.LoadAssetAtPath<AudioClip>("Assets/Sounds/alphabet/i.mp3")},
            {'j', AssetDatabase.LoadAssetAtPath<AudioClip>("Assets/Sounds/alphabet/j.mp3")},
            {'k', AssetDatabase.LoadAssetAtPath<AudioClip>("Assets/Sounds/alphabet/k.mp3")},
            {'l', AssetDatabase.LoadAssetAtPath<AudioClip>("Assets/Sounds/alphabet/l.mp3")},
            {'m', AssetDatabase.LoadAssetAtPath<AudioClip>("Assets/Sounds/alphabet/m.mp3")},
            {'n', AssetDatabase.LoadAssetAtPath<AudioClip>("Assets/Sounds/alphabet/n.mp3")},
            {'o', AssetDatabase.LoadAssetAtPath<AudioClip>("Assets/Sounds/alphabet/o.mp3")},
            {'p', AssetDatabase.LoadAssetAtPath<AudioClip>("Assets/Sounds/alphabet/p.mp3")},
            {'q', AssetDatabase.LoadAssetAtPath<AudioClip>("Assets/Sounds/alphabet/q.mp3")},
            {'r', AssetDatabase.LoadAssetAtPath<AudioClip>("Assets/Sounds/alphabet/r.mp3")},
            {'s', AssetDatabase.LoadAssetAtPath<AudioClip>("Assets/Sounds/alphabet/s.mp3")},
            {'t', AssetDatabase.LoadAssetAtPath<AudioClip>("Assets/Sounds/alphabet/t.mp3")},
            {'u', AssetDatabase.LoadAssetAtPath<AudioClip>("Assets/Sounds/alphabet/u.mp3")},
            {'v', AssetDatabase.LoadAssetAtPath<AudioClip>("Assets/Sounds/alphabet/v.mp3")},
            {'w', AssetDatabase.LoadAssetAtPath<AudioClip>("Assets/Sounds/alphabet/w.mp3")},
            {'x', AssetDatabase.LoadAssetAtPath<AudioClip>("Assets/Sounds/alphabet/x.mp3")},
            {'y', AssetDatabase.LoadAssetAtPath<AudioClip>("Assets/Sounds/alphabet/y.mp3")},
            {'z', AssetDatabase.LoadAssetAtPath<AudioClip>("Assets/Sounds/alphabet/z.mp3")},
            
            // Numbers
            {'0', AssetDatabase.LoadAssetAtPath<AudioClip>("Assets/Sounds/numbers/0.mp3")},
            {'1', AssetDatabase.LoadAssetAtPath<AudioClip>("Assets/Sounds/numbers/1.mp3")},
            {'2', AssetDatabase.LoadAssetAtPath<AudioClip>("Assets/Sounds/numbers/2.mp3")},
            {'3', AssetDatabase.LoadAssetAtPath<AudioClip>("Assets/Sounds/numbers/3.mp3")},
            {'4', AssetDatabase.LoadAssetAtPath<AudioClip>("Assets/Sounds/numbers/4.mp3")},
            {'5', AssetDatabase.LoadAssetAtPath<AudioClip>("Assets/Sounds/numbers/5.mp3")},
            {'6', AssetDatabase.LoadAssetAtPath<AudioClip>("Assets/Sounds/numbers/6.mp3")},
            {'7', AssetDatabase.LoadAssetAtPath<AudioClip>("Assets/Sounds/numbers/7.mp3")},
            {'8', AssetDatabase.LoadAssetAtPath<AudioClip>("Assets/Sounds/numbers/8.mp3")},
            {'9', AssetDatabase.LoadAssetAtPath<AudioClip>("Assets/Sounds/numbers/9.mp3")},
            
            // Other
            {',', AssetDatabase.LoadAssetAtPath<AudioClip>("Assets/Sounds/other/comma.mp3")},
            {'.', AssetDatabase.LoadAssetAtPath<AudioClip>("Assets/Sounds/other/period.mp3")},
            {'!', AssetDatabase.LoadAssetAtPath<AudioClip>("Assets/Sounds/other/exclamationMark.mp3")},
            {'?', AssetDatabase.LoadAssetAtPath<AudioClip>("Assets/Sounds/other/questionMark.mp3")},
        };
    }
    public void PlaySound(AudioClip sound, SOUND_TYPE soundType)
    {
        //Debug.Log($"PlaySound({sound}, {soundType})");

        //Determine volume
        //Debug.Log($"gameManager: {gameManager}");
        //Debug.Log($"gameManager.masterVolume: {gameManager.masterVolume}");
        //Debug.Log($"gameManager.soundSettings[soundType]: {gameManager.GetVolume(soundType)}");
        float volume = gameManager.masterVolume * gameManager.GetVolume(soundType);
        //Debug.Log($"volume: {volume}");

        //Determine speed and play sound
        //Debug.Log($"SOUND_TYPE.VOICE: {SOUND_TYPE.VOICE}");
        if (soundType == SOUND_TYPE.VOICE)
        {
            //Debug.Log($"gameManager.narratorSpeed: {gameManager.narratorSpeed}");
            PlaySound(sound, volume, gameManager.narratorSpeed, soundType);
        }
        else
        {
            PlaySound(sound, volume, 1f, soundType);
        }
    }
    public void PlayCharacterAudio(char character)
    {
        character = char.ToLower(character);
        if (character == ' ' || character == '\n')
        {
            return;
        }
        if (audioDictionary.ContainsKey(character))
        {
            PlaySound(audioDictionary[character], SOUND_TYPE.VOICE);
        }
        else
        {
            Debug.LogError($"ERROR: No audio found for '{character}'!");
        }
    }
    public void PlaySound(AudioClip sound, float volume, float speed, SOUND_TYPE soundType)
    {
        ////Debug.Log($"PlaySound({sound}, {volume}, {speed})");
        AudioSource audioSource = GetAvailableSource(soundType);
        ////Debug.Log($"audioSource: {audioSource}");
        audioSource.resource = sound;
        audioSource.volume = volume;
        audioSource.pitch = speed;
        if (soundType == SOUND_TYPE.GAME)
        {
            audioSource.outputAudioMixerGroup = gameMixer;
        }
        else if (soundType == SOUND_TYPE.VOICE)
        {
            audioSource.outputAudioMixerGroup = voiceMixer;
        }
        audioSource.Play();
    }
    public AudioSource GetAvailableSource(SOUND_TYPE soundType)
    {
        ////Debug.Log($"audioSources: {audioSources}");
        ////Debug.Log($"audioSources.Count: {audioSources.Count}");
        for (int i = 0; i < audioSources.Count; i++)
        {
            AudioSource audioSource = audioSources.ElementAt(i).Key;
            if (!audioSource.isPlaying)
            {
                return audioSource;
            }
        }
        // No available sources
        if (audioSources.Count >= maxChannels)
        {
            // Use the one closest to ending
            AudioSource bestSource = null;
            float shortestTime = -1;
            for (int i = 0; i < audioSources.Count; i++)
            {
                AudioSource audioSource = audioSources.ElementAt(i).Key;
                float secLeft = ((AudioClip)audioSource.resource).length - audioSource.time;
                if (secLeft < shortestTime || shortestTime == -1)
                {
                    shortestTime = secLeft;
                    bestSource = audioSource;
                }
            }
            return bestSource;
        }
        else
        {
            // Make a new one
            AudioSource newSource = transform.AddComponent<AudioSource>();
            audioSources.Add(newSource, soundType);
            return newSource;
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
        audioSources = new Dictionary<AudioSource, SOUND_TYPE>();
        DontDestroyOnLoad(gameObject);

        CreateDict();
    }
    void Start()
    {
        gameManager = GameManager.Instance;

        gameMixer = (Resources.Load("Game") as AudioMixer).FindMatchingGroups("Master")[0];
        voiceMixer = (Resources.Load("Voice") as AudioMixer).FindMatchingGroups("Master")[0];
    }
    void Update()
    {
        // Audio Queue
        if (audioQueuePlaying)
        {
            if (audioQueue.Count <= 0)
            {
                // Queue is over
                audioQueuePlaying = false;
            }
            else if (delayTimer <= 0)
            {
                // Play sound
                PlaySound(audioQueue[0], SOUND_TYPE.VOICE);
                OnSoundSettingsUpdate();
                audioQueue.RemoveAt(0);
                delayTimer = secondsBetweenSounds;
            }
            else
            {
                delayTimer -= Time.deltaTime;
            }
        }
    }
}
