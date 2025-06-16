using UnityEngine;

public class AudioController : MonoBehaviour
{
    public static AudioController Instance;

    [Header("Audio Clips")]
    public AudioClip interactSFX;
    public AudioClip damageSFX;
    public AudioClip deathSFX;
    public AudioClip victorySFX;
    public AudioClip bgm;

    private AudioSource sfxSource;
    private AudioSource musicSource;

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

    public void PlaySFX(AudioClip clip)
    {
        if (clip != null)
            sfxSource.PlayOneShot(clip);
    }

    public void PlayVictory()
    {
        musicSource.Stop();
        sfxSource.PlayOneShot(victorySFX);
    }

    public void PlayDeath()
    {
        musicSource.Stop();
        sfxSource.PlayOneShot(deathSFX);
    }
}
