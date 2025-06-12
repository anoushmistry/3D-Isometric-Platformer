using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class LightReceiver : MonoBehaviour
{
    [SerializeField] private Material material;
    [SerializeField] private float targetIntensity = 5f;
    [SerializeField] private float lerpDuration = 4f;
    private Coroutine emissionCoroutine;
    private Color baseEmissionColor;

    private void Start()
    {
        // Store the base emission color (this assumes it’s already set in the material)
        baseEmissionColor = material.GetColor("_EmissionColor");
    }
    public void Activate()
    {

        if (emissionCoroutine != null)
            StopCoroutine(emissionCoroutine);

        emissionCoroutine = StartCoroutine(LerpEmissionIntensity(0f, targetIntensity, lerpDuration));
        Debug.Log("Laser Receiver Activated!");
        // Add your logic here, e.g., open doors, trigger lights, etc.
    }
    private IEnumerator LerpEmissionIntensity(float startIntensity, float endIntensity, float duration)
    {
        float elapsed = 0f;

        material.EnableKeyword("_EMISSION");

        while (elapsed < duration)
        {
            float t = elapsed / duration;
            float currentIntensity = Mathf.Lerp(startIntensity, endIntensity, t);
            material.SetColor("_EmissionColor", baseEmissionColor * currentIntensity);
            elapsed += Time.deltaTime;
            yield return null;
        }

        // Ensure final intensity is applied
        material.SetColor("_EmissionColor", baseEmissionColor * endIntensity);
    }
}
