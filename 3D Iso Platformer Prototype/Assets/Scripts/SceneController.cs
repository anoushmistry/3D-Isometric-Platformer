using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

public class SceneController : MonoBehaviour
{
    public static SceneController Instance;

    [Header("Fade Settings")]
    [SerializeField] private Image fadeImage;
    [SerializeField] private float fadeDuration = 1f;

    private bool isFading = false;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
        DontDestroyOnLoad(fadeImage.transform.parent.gameObject);
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

        // Load scene (replaces current scene)
        AsyncOperation loadOp = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Single);
        while (!loadOp.isDone)
            yield return null;

        // Fade In
        yield return StartCoroutine(Fade(0));

        isFading = false;
    }

    private IEnumerator Fade(float targetAlpha)
    {
        if (fadeImage == null) yield break;

        fadeImage.raycastTarget = true;
        Color color = fadeImage.color;
        float startAlpha = color.a;
        float timer = 0f;

        while (timer < fadeDuration)
        {
            float t = timer / fadeDuration;
            color.a = Mathf.Lerp(startAlpha, targetAlpha, t);
            fadeImage.color = color;
            timer += Time.deltaTime;
            yield return null;
        }

        color.a = targetAlpha;
        fadeImage.color = color;

        fadeImage.raycastTarget = targetAlpha > 0;
    }
}
