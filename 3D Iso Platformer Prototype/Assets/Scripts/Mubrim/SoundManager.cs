using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance;

    [Header("Environment Sound")]
    public AudioSource environmentSource;
    [Range(0f, 1f)] public float environmentVolume = 1f;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void PlayEnvironmentSound(AudioClip clip)
    {
        if (environmentSource == null || clip == null)
        {
            Debug.LogWarning("Missing AudioSource or AudioClip!");
            return;
        }

        if (environmentSource.clip == clip && environmentSource.isPlaying) return;

        environmentSource.clip = clip;
        environmentSource.loop = true;
        environmentSource.volume = environmentVolume;
        environmentSource.Play();
    }


    public void StopEnvironmentSound()
    {
        if (environmentSource != null)
            environmentSource.Stop();
    }

    public void SetEnvironmentVolume(float volume)
    {
        environmentVolume = volume;
        if (environmentSource != null)
            environmentSource.volume = volume;
    }
}
