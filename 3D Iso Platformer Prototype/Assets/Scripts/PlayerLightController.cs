using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLightController : MonoBehaviour
{
    [SerializeField] private Light playerLight;

    [SerializeField] private float intensityChangeSpeed;
    private Material lightMaterial;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKey(KeyCode.Mouse0))
        {
            playerLight.intensity = Mathf.Lerp(playerLight.intensity,2, intensityChangeSpeed * Time.deltaTime);
        }
        else
        {
            playerLight.intensity = Mathf.Lerp(playerLight.intensity, 0, intensityChangeSpeed * Time.deltaTime);
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "LightObject")
        {
           Material lightMat =  other.GetComponent<Renderer>().material;
            Light light = other.GetComponent<Light>();
            StartCoroutine(TakeOrb(lightMat,light,2f));
        }
    }
    IEnumerator TakeOrb(Material mat, Light light, float seconds)
    {
        float elapsedTime = 0f;
        float startIntensity = mat.GetFloat("_Intensity");
        float startLightIntensity = light.intensity;

        while (elapsedTime < seconds)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / seconds; // Normalized time (0 to 1)

            // Lerp the values over time
            mat.SetFloat("_Intensity", Mathf.Lerp(startIntensity, 0, t));
            light.intensity = Mathf.Lerp(startLightIntensity, 0f, t);

            yield return null; // Wait for next frame
        }

        // Ensure values are exactly zero at the end
        mat.SetFloat("_Intensity", 0);
        light.intensity = 0;
    }
}
