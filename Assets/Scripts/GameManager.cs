using System.Linq;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    // Global settings
    public float masterVolume = 1f;
    public float voiceVolume = 1f;
    public float gameVolume = 1f;
    public bool narratorOn = true;

    // Copy game
    public bool letterMode = true;
    public bool showCopyCell = true;

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
}
