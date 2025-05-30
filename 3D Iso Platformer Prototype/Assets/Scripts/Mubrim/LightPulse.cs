using UnityEngine;

[RequireComponent(typeof(Light))]
public class LightPulse : MonoBehaviour
{
    public float minIntensity = 2f;       
    public float maxIntensity = 4f;      
    public float pulseSpeed = 2f;   

    private Light orbLight;

    void Start()
    {
        orbLight = GetComponent<Light>();
    }

    void Update()
    {
        float pulse = Mathf.Sin(Time.time * pulseSpeed) * 0.5f + 0.5f; 
        orbLight.intensity = Mathf.Lerp(minIntensity, maxIntensity, pulse);
    }
}
