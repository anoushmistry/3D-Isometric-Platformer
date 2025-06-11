using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

public class SceneController : MonoBehaviour
{
    public static SceneController Instance;

    [Header("Fade Settings")]
    public Image fadeImageWhite;
    [SerializeField] private float fadeDuration = 1f;
    [Header("Ambient Clips")]
    public AudioClip suspenseAmbientClip;
    public AudioClip natureAmbientClip;



    private bool isFading = false;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(this.gameObject);
    }

    public void LoadScene(string sceneName)
    {
        if (!isFading)
            StartCoroutine(FadeAndLoad(sceneName));
    }

    private IEnumerator FadeAndLoad(string sceneName)
    {
        isFading = true;

        // Fade Out
        yield return StartCoroutine(Fade(1));

        // Load scene
        AsyncOperation loadOp = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Single);
        while (!loadOp.isDone)
            yield return null;

        // ✅ Play ambient based on scene
        if (SoundManager.Instance != null)
        {
            string lowerName = sceneName.ToLower();

            if (lowerName.Contains("tutorial level"))
            {
                SoundManager.Instance.PlayEnvironmentSound(suspenseAmbientClip);
            }
            else
            {
                SoundManager.Instance.PlayEnvironmentSound(natureAmbientClip);
            }
        }

        // Fade In
        yield return StartCoroutine(Fade(0));

        isFading = false;
    }



    private IEnumerator Fade(float targetAlpha)
    {
        if (fadeImageWhite == null) yield break;

        fadeImageWhite.raycastTarget = true;
        Color color = fadeImageWhite.color;
        float startAlpha = color.a;
        float timer = 0f;

        while (timer < fadeDuration)
        {
            float t = timer / fadeDuration;
            color.a = Mathf.Lerp(startAlpha, targetAlpha, t);
            fadeImageWhite.color = color;
            timer += Time.deltaTime;
            yield return null;
        }

        color.a = targetAlpha;
        fadeImageWhite.color = color;
        fadeImageWhite.raycastTarget = targetAlpha > 0;
    }
}
