using UnityEngine;

[RequireComponent(typeof(Light))]
public class LightPulse : MonoBehaviour
{
    public float minIntensity = 2f;       // Minimum glow
    public float maxIntensity = 4f;       // Maximum glow
    public float pulseSpeed = 2f;         // How fast it pulses

    private Light orbLight;

    void Start()
    {
        orbLight = GetComponent<Light>();
    }

    void Update()
    {
        float pulse = Mathf.Sin(Time.time * pulseSpeed) * 0.5f + 0.5f; // Range: 0 to 1
        orbLight.intensity = Mathf.Lerp(minIntensity, maxIntensity, pulse);
    }
}
