using UnityEngine;
using UnityEngine.SceneManagement;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance;

    [Header("Audio Setup")]
    public AudioSource environmentSource;
    public float environmentVolume = 1f;

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
    }

    private void Start()
    {
        PlaySceneMusic(SceneManager.GetActiveScene().name);
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        PlaySceneMusic(scene.name);
    }

    public void PlaySceneMusic(string sceneName)
    {
        if (environmentSource == null) return;

        AudioClip clipToPlay = null;

        switch (sceneName)
        {
            case "MainMenu":
                clipToPlay = mainMenuClip;
                environmentVolume = 0.1f;
                break;
            case "Tutorial Level":
                clipToPlay = tutorialClip;
                environmentVolume = 0.005f;
                break;
            case "Level 1 Prototype":
                clipToPlay = level1Clip;
                environmentVolume = 0.1f;
                break;
            case "Level 2 Prototype":
                clipToPlay = level2Clip;
                break;
            default:
                clipToPlay = null;
                break;
        }

        if (clipToPlay != null && environmentSource.clip != clipToPlay)
        {
            environmentSource.Stop();
            environmentSource.clip = clipToPlay;
            environmentSource.volume = environmentVolume;
            environmentSource.loop = true;
            environmentSource.Play();
        }
    }
}
