using UnityEngine;
using System.Collections.Generic;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance;

    [Header("Audio Sources")]
    public AudioSource musicSource;
    public AudioSource musicSourceSecondary; // for crossfade, optional
    public GameObject sfxSourcePrefab;
    public int sfxPoolSize = 10;

    [Header("Volume Settings")]
    [Range(0f, 1f)] public float musicVolume = 1f;
    [Range(0f, 1f)] public float sfxVolume = 1f;

    private List<AudioSource> sfxSources = new List<AudioSource>();
    private int currentSfxIndex = 0;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            InitializeSfxPool();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void InitializeSfxPool()
    {
        for (int i = 0; i < sfxPoolSize; i++)
        {
            GameObject sfxObj = Instantiate(sfxSourcePrefab, transform);
            AudioSource src = sfxObj.GetComponent<AudioSource>();
            src.playOnAwake = false;
            sfxSources.Add(src);
        }
    }

    // === MUSIC ===
    public void PlayMusic(AudioClip clip, bool loop = true)
    {
        if (musicSource.isPlaying && musicSource.clip == clip) return;

        musicSource.clip = clip;
        musicSource.loop = loop;
        musicSource.volume = musicVolume;
        musicSource.Play();
    }

    public void StopMusic()
    {
        musicSource.Stop();
    }

    public void SetMusicVolume(float volume)
    {
        musicVolume = volume;
        musicSource.volume = musicVolume;
    }

    // === SOUND EFFECTS ===
    public void PlaySFX(AudioClip clip, float pitchRandom = 0f)
    {
        AudioSource src = sfxSources[currentSfxIndex];
        currentSfxIndex = (currentSfxIndex + 1) % sfxSources.Count;

        src.clip = clip;
        src.volume = sfxVolume;
        src.pitch = 1f + Random.Range(-pitchRandom, pitchRandom);
        src.Play();
    }

    public void SetSFXVolume(float volume)
    {
        sfxVolume = volume;
    }
}
