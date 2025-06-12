using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(BoxCollider))]
public class LightReceiver : MonoBehaviour
{
    [SerializeField] private Material material;
    [SerializeField] private float targetIntensity = 5f;
    [SerializeField] private float lerpDuration = 4f;
    private Coroutine emissionCoroutine;
    private Color baseEmissionColor;
    private void Awake()
    {
       // material = GetComponent<Renderer>().material; // Clone instance for this object
    }
    private void Start()
    {
        // Store the base emission color (this assumes it’s already set in the material)
       // baseEmissionColor = material.GetColor("_EmissionColor");
    }
    public void Activate()
    {
        SceneController.Instance.LoadScene("EndScreen");
        UnlockCursor();


     //   if (material == null) return;
     //  material.EnableKeyword("_EMISSION");

        //  if (emissionCoroutine != null)
        //    StopCoroutine(emissionCoroutine);

        // emissionCoroutine = StartCoroutine(LerpEmissionIntensity(0f, targetIntensity, lerpDuration));
        Debug.Log("Laser Receiver Activated!");
        // Add your logic here, e.g., open doors, trigger lights, etc.
    }
    //private IEnumerator LerpEmissionIntensity(float startIntensity, float endIntensity, float duration)
    //{
    //    float elapsed = 0f;

    //    while (elapsed < duration)
    //    {
    //        float t = elapsed / duration;
    //        float currentIntensity = Mathf.Lerp(startIntensity, endIntensity, t);
    //        material.SetColor("_EmissionColor", baseEmissionColor * currentIntensity);
    //        elapsed += Time.deltaTime;
    //        yield return null;
    //    }

    //    // Ensure final intensity is applied
    //    material.SetColor("_EmissionColor", baseEmissionColor * endIntensity);
    //}

    //private IEnumerator Fade(float targetAlpha)
    //{
    //    if (fadeImageYellow == null) yield break;

    //    fadeImageYellow.raycastTarget = true;
    //    Color color = fadeImageYellow.color;
    //    float startAlpha = color.a;
    //    float timer = 0f;

    //    while (timer < fadeDuration)
    //    {
    //        float t = timer / fadeDuration;
    //        color.a = Mathf.Lerp(startAlpha, targetAlpha, t);
    //        fadeImageYellow.color = color;
    //        timer += Time.deltaTime;
    //        yield return null;
    //    }

    //    color.a = targetAlpha;
    //    fadeImageYellow.color = color;
    //    fadeImageYellow.raycastTarget = targetAlpha > 0;
    //}
    public void UnlockCursor()
    {
        Cursor.lockState = CursorLockMode.None; // Unlocks the cursor
        Cursor.visible = true; // Shows the cursor
    }
}
