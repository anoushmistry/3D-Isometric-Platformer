using UnityEngine;
using UnityEngine.SceneManagement;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance;

    [Header("Audio Sources")]
    public AudioSource environmentSource;
    public AudioSource sfxSource;

    [Header("Volume Controls")]
    [Range(0f, 1f)] public float musicVolume = 1f;
    [Range(0f, 1f)] public float sfxVolume = 1f;

    [Header("SFX Clips")]
    public AudioClip footstepClip;
    public AudioClip climbClip;
    public AudioClip leverClip;
    public AudioClip gateDropClip;
    public AudioClip doorBangClip;
    public AudioClip orbPickupClip;
    public AudioClip orbPlaceClip;
    public AudioClip fallingTreeClip;
    public AudioClip portalClip;

    [Header("SFX Sources")]
    public AudioSource footstepSource;
    public AudioSource climbSource;

    [Header("Scene Clips")]
    public AudioClip mainMenuClip;
    public AudioClip tutorialClip;
    public AudioClip level1Clip;
    public AudioClip level2Clip;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
        SceneManager.sceneLoaded += OnSceneLoaded;
        musicVolume = 0.6f;
    }

    private void Start()
    {
        ApplyVolumes();
        PlaySceneMusic(SceneManager.GetActiveScene().name);
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        PlaySceneMusic(scene.name);
    }

    public void PlaySceneMusic(string sceneName)
    {
        if (environmentSource == null) return;

        AudioClip clipToPlay = sceneName switch
        {
            "MainMenu" => mainMenuClip,
            "Tutorial Level" => tutorialClip,
            "Level 1 Prototype" => level1Clip,
            "Level 2 Prototype" => level2Clip,
            _ => null
        };

        if (clipToPlay != null && environmentSource.clip != clipToPlay)
        {
            environmentSource.Stop();
            environmentSource.clip = clipToPlay;
            environmentSource.volume = musicVolume;
            environmentSource.loop = true;
            environmentSource.Play();
            ApplyVolumes();
        }
    }

    public void PlaySFX(AudioClip clip)
    {
        if (clip == null || sfxSource == null) return;
        sfxSource.PlayOneShot(clip, sfxVolume);
    }

    public void PlayLeverSFX(Vector3 position)
    {
        if (leverClip == null) return;
        AudioSource.PlayClipAtPoint(leverClip, position, sfxVolume);
    }

    public void PlayGateDropSFX(Vector3 position)
    {
        if (gateDropClip == null) return;
        AudioSource.PlayClipAtPoint(gateDropClip, position, sfxVolume);
    }

    public void PlayDoorBangSFX(Vector3 position)
    {
        if (doorBangClip == null) return;
        AudioSource.PlayClipAtPoint(doorBangClip, position, sfxVolume);
    }

    public void PlayFootstepLoop()
    {
        if (footstepClip == null || footstepSource == null || footstepSource.isPlaying) return;

        footstepSource.clip = footstepClip;
        footstepSource.pitch = 1.5f;
        footstepSource.loop = true;
        footstepSource.volume = sfxVolume;
        footstepSource.Play();
    }

    public void StopFootstepLoop()
    {
        if (footstepSource != null && footstepSource.isPlaying)
            footstepSource.Stop();
    }

    public void PlayClimbLoop()
    {
        if (climbClip == null || climbSource == null || climbSource.isPlaying) return;

        climbSource.clip = climbClip;
        climbSource.pitch = 1f;
        climbSource.loop = true;
        climbSource.volume = sfxVolume;
        climbSource.Play();
    }

    public void StopClimbLoop()
    {
        if (climbSource != null && climbSource.isPlaying)
            climbSource.Stop();
    }
    public void PlayPortalSFX() 
    {
        if (portalClip == null || sfxSource == null) return;
        sfxSource.PlayOneShot(portalClip, sfxVolume);
    }

    public void PlayOrbPickupSFX()
    {
        if (orbPickupClip == null || sfxSource == null) return;
        sfxSource.PlayOneShot(orbPickupClip, sfxVolume);
    }

    public void PlayOrbPlaceSFX()
    {
        if (orbPlaceClip == null || sfxSource == null) return;
        sfxSource.PlayOneShot(orbPlaceClip, sfxVolume);
    }

    public void PlayFallingTreeSFX() 
    {
        if (fallingTreeClip == null || sfxSource == null) return;
        sfxSource.PlayOneShot(fallingTreeClip, sfxVolume);
    }

    public void SetEnvironmentVolume(float volume)
    {
        musicVolume = volume;
        ApplyVolumes();
    }

    public void SetSFXVolume(float volume)
    {
        sfxVolume = volume;
        ApplyVolumes();
    }

    public void SetMusicVolume(float volume)
    {
        musicVolume = volume;
        if (environmentSource != null)
            environmentSource.volume = musicVolume;
    }

    private void ApplyVolumes()
    {
        if (footstepSource != null) footstepSource.volume = sfxVolume;
        if (climbSource != null) climbSource.volume = sfxVolume;
        if (sfxSource != null) sfxSource.volume = sfxVolume;
        if (environmentSource != null) environmentSource.volume = musicVolume;
    }
}
