using UnityEngine;

public class MusicManager : MonoBehaviour
{
    public AudioSource backgroundMusic;
    private static MusicManager instance;

    void Awake()
    {
        // Singleton pattern: If instance already exists, destroy the new one
        if (instance != null)
        {
            Destroy(gameObject); // Destroy duplicate instance
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(gameObject); // Keep the MusicManager across scene loads
        }
    }

    void Start()
    {
        if (backgroundMusic != null && !backgroundMusic.isPlaying)
        {
            backgroundMusic.Play(); // Play the background music if not already playing
        }
    }
}
