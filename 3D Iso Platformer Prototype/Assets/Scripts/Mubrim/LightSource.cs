using UnityEngine;
using DG.Tweening;

public class LightSource : MonoBehaviour
{
    private Light pointLight;
    private bool isOn = false;
    public GameObject lightBeam; // Assign LightBeam in the Inspector

    void Start()
    {
        pointLight = GetComponentInChildren<Light>();
        pointLight.intensity = 0;  // Start with light OFF
        if (lightBeam) lightBeam.SetActive(false); // Ensure light beam starts disabled
    }

    public void ToggleLight()
    {
        isOn = !isOn;
        float targetIntensity = isOn ? 2f : 0f;
        pointLight.DOIntensity(targetIntensity, 0.5f);

        if (lightBeam)
            lightBeam.SetActive(isOn); // Enable or disable the beam
    }
}
