using UnityEngine;
using TMPro;
using System.Collections;
using Unity.VisualScripting;

public class TextFade : MonoBehaviour
{
    public TextMeshProUGUI textToFade;
    public float fadeInDuration = 2.0f;
    public float fadeOutDelay = 5.0f; // Time before fade-out starts
    public float fadeOutDuration = 2.0f;
    private bool hasFaded = false;

    [SerializeField] private bool FadeInThenFadeOutAutomatically;
    private Coroutine _currentFadeRoutine;
    void Start()
    {
        if (textToFade != null)
        {
            // Initialize both the text color AND material color
            Color textColor = textToFade.color;
            textColor.a = 0f;
            textToFade.color = textColor;

            // Create material instance to avoid modifying shared material
            textToFade.fontMaterial = new Material(textToFade.fontMaterial);

            // Set initial material alpha
            Color faceColor = textToFade.fontMaterial.GetColor("_FaceColor");
            faceColor.a = 0f;
            textToFade.fontMaterial.SetColor("_FaceColor", faceColor);
        }
    }

    void OnTriggerEnter(Collider other)
    {

        if (other.CompareTag("Player") && !hasFaded)
        {
            if (FadeInThenFadeOutAutomatically)
            {
                StartCoroutine(FadeInThenOut());
                hasFaded = true;
            }
            else
            {
                FadeIn();
                hasFaded = true;
            }
        }
    }

    public void FadeIn()
    {
        FadeIn(fadeInDuration);
    }

    public void FadeIn(float duration)
    {
        StartCoroutine(FadeTextRoutine(0f, 1f, duration));
    }
    public void FadeOut()
    {
        FadeOut(fadeOutDuration);
    }

    public void FadeOut(float duration)
    {
        StartCoroutine(FadeTextRoutine(1f, 0f, duration));
    }
    IEnumerator FadeInThenOut()
    {
        // First fade in
        yield return StartCoroutine(FadeTextRoutine(0f, 1f, fadeInDuration));

        // Wait for the specified delay before fading out
        yield return new WaitForSeconds(fadeOutDelay);

        // Then fade out
        yield return StartCoroutine(FadeTextRoutine(1f, 0f, fadeOutDuration));
    }

    //IEnumerator FadeText(float startAlpha, float endAlpha, float duration)
    //{
    //    float elapsedTime = 0f;
    //    Material textMat = textToFade.fontMaterial;

    //    // Get original colors (without alpha)
    //    Color originalTextColor = textToFade.color;
    //    Color originalFaceColor = textMat.GetColor("_FaceColor");
    //    Color originalGlowColor = textMat.HasProperty("_GlowColor") ?
    //        textMat.GetColor("_GlowColor") : Color.clear;

    //    while (elapsedTime < duration)
    //    {
    //        elapsedTime += Time.deltaTime;
    //        float progress = Mathf.Clamp01(elapsedTime / duration);
    //        float currentAlpha = Mathf.Lerp(startAlpha, endAlpha, progress);

    //        // Update text color alpha
    //        Color currentTextColor = originalTextColor;
    //        currentTextColor.a = currentAlpha;
    //        textToFade.color = currentTextColor;

    //        // Update face color alpha
    //        Color currentFaceColor = originalFaceColor;
    //        currentFaceColor.a = currentAlpha;
    //        textMat.SetColor("_FaceColor", currentFaceColor);

    //        // Update glow color alpha if exists
    //        if (textMat.HasProperty("_GlowColor"))
    //        {
    //            Color currentGlowColor = originalGlowColor;
    //            currentGlowColor.a = currentAlpha;
    //            textMat.SetColor("_GlowColor", currentGlowColor);
    //        }

    //        yield return null;
    //    }

    //    // Ensure final values
    //    originalTextColor.a = endAlpha;
    //    textToFade.color = originalTextColor;

    //    originalFaceColor.a = endAlpha;
    //    textMat.SetColor("_FaceColor", originalFaceColor);

    //    if (textMat.HasProperty("_GlowColor"))
    //    {
    //        originalGlowColor.a = endAlpha;
    //        textMat.SetColor("_GlowColor", originalGlowColor);
    //    }
    //}
    private IEnumerator FadeTextRoutine(float startAlpha, float endAlpha, float duration)
    {
        float elapsedTime = 0f;
        SetTextAlpha(startAlpha);

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float progress = Mathf.Clamp01(elapsedTime / duration);
            float currentAlpha = Mathf.Lerp(startAlpha, endAlpha, progress);
            SetTextAlpha(currentAlpha);
            yield return null;
        }

        SetTextAlpha(endAlpha);
        _currentFadeRoutine = null;
    }

    private void SetTextAlpha(float alpha)
    {
        // Set text color alpha
        Color textColor = textToFade.color;
        textColor.a = alpha;
        textToFade.color = textColor;

        Material _textMaterial = textToFade.fontMaterial;
        // Set face color alpha
        Color faceColor = _textMaterial.GetColor("_FaceColor");
        faceColor.a = alpha;
        _textMaterial.SetColor("_FaceColor", faceColor);

        // Set glow color alpha if exists
        if (_textMaterial.HasProperty("_GlowColor"))
        {
            Color glowColor = _textMaterial.GetColor("_GlowColor");
            glowColor.a = alpha;
            _textMaterial.SetColor("_GlowColor", glowColor);
        }
    }
}