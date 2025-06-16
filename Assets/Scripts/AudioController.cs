using UnityEngine;

/// Controls all audio playback including background music and sound effects.
/// Implements a singleton pattern for global access.
public class AudioController : MonoBehaviour
{
    /// Singleton instance of the AudioController.
    public static AudioController Instance;

    /// Audio clip played when player interacts with an object.
    [Header("Audio Clips")]
    public AudioClip interactSFX;

    /// Audio clip played when player takes damage.
    public AudioClip damageSFX;

    /// Audio clip played when player dies.
    public AudioClip deathSFX;

    /// Audio clip played on victory.
    public AudioClip victorySFX;

    /// Background music audio clip.
    public AudioClip bgm;

    /// AudioSource component for playing sound effects.
    private AudioSource sfxSource;

    /// AudioSource component for playing background music.
    private AudioSource musicSource;

    /// Awake is called when the script instance is being loaded.
    /// Sets up the singleton instance and initializes AudioSources.
    void Awake()
    {
        // Singleton setup
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);

            // Setup 2 AudioSources: one for music, one for SFX
            sfxSource = gameObject.AddComponent<AudioSource>();
            musicSource = gameObject.AddComponent<AudioSource>();
            musicSource.loop = true;
            musicSource.clip = bgm;
            musicSource.volume = 0.5f;
            musicSource.Play();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    /// Plays a one-shot sound effect clip.
    /// <param name="clip">The AudioClip to play.</param>
    public void PlaySFX(AudioClip clip)
    {
        if (clip != null)
            sfxSource.PlayOneShot(clip);
    }

    /// Stops background music and plays the victory sound effect.
    public void PlayVictory()
    {
        musicSource.Stop();
        sfxSource.PlayOneShot(victorySFX);
    }

    /// Stops background music and plays the death sound effect.
    public void PlayDeath()
    {
        musicSource.Stop();
        sfxSource.PlayOneShot(deathSFX);
    }
}
