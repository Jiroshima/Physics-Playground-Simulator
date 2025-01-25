using UnityEngine;

public class CollisionSound : MonoBehaviour
{
    private AudioSource audioSource;  // AudioSource component to play sound
    private AudioClip collisionSound;  // Sound to play on collision

    [SerializeField] private float dopplerLevel = 1f;  // Doppler effect intensity
    [SerializeField] private float cooldownTime = 1f;  // Cooldown time between sounds (in seconds)

    private float lastSoundTime = 0f;  // Time of last sound played

    private void Start()
    {
        // Ensure we have an AudioSource component
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();  // Add one if not present
        }

        // Make sure audio is spatial
        audioSource.spatialBlend = 1f;  // 3D sound
        audioSource.loop = false;  // Don't loop sound by default
        audioSource.dopplerLevel = dopplerLevel;  // Set Doppler effect
    }

    public void SetCollisionSound(AudioClip sound)
    {
        collisionSound = sound;
    }

    // Method to play sound with volume and 3D adjustments
    public void PlaySound(float volume, bool is3D)
    {
        // Only play sound if enough time has passed since the last sound
        if (collisionSound != null && audioSource != null && Time.time > lastSoundTime + cooldownTime)
        {
            audioSource.PlayOneShot(collisionSound, volume);  // Play sound with given volume
            lastSoundTime = Time.time;  // Update last sound time to current time
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        // Only play sound if cooldown has passed
        if (collisionSound != null && audioSource != null && Time.time > lastSoundTime + cooldownTime)
        {
            // Play the collision sound with reduced volume for collisions
            audioSource.PlayOneShot(collisionSound, 0.5f);  // Adjust volume as needed
            lastSoundTime = Time.time;  // Update the last sound time to prevent further plays too soon
        }
    }
}
