using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[ExecuteInEditMode]
public class WeatherManager : MonoBehaviour
{

    public Vector3 WindDirection;

    public float WindIntensityMain;
    public float WindIntensitySecondary;
    public float WindTurbulence;

    void Update()
    {
        Vector3 wind = Vector3.Normalize(WindDirection);
        Vector4 shaderValue = new Vector4(wind.x, wind.y, wind.z, 0.0f); 

        Shader.SetGlobalVector("_WindDirection", shaderValue);

        Shader.SetGlobalFloat("_WindIntensityMain", WindIntensityMain);
        Shader.SetGlobalFloat("_WindIntensitySecondary", WindIntensitySecondary);
        Shader.SetGlobalFloat("_WindTurbulence", WindTurbulence);
    }
}
