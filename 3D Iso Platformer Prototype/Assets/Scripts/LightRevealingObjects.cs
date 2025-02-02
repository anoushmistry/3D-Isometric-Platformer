using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightRevealingObjects : MonoBehaviour
{
    private Renderer rend;
    private float fadeSpeed = 2f;
    private bool isLit = false;

    void Start()
    {
        rend = GetComponent<MeshRenderer>();
        SetAlpha(0); // Start fully invisible
    }

    void Update()
    {
        float targetAlpha = isLit ? 1f : 0f;
        Color color = rend.material.color;
        color.a = Mathf.Lerp(color.a, targetAlpha, fadeSpeed * Time.deltaTime);
        rend.material.color = color;
    }

    private void SetAlpha(float alpha)
    {
        Color color = rend.material.color;
        color.a = alpha;
        rend.material.color = color;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Light"))
        {
            isLit = true;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Light"))
        {
            isLit = false;
        }
    }
}
