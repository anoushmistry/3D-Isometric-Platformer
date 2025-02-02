using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLightController : MonoBehaviour
{
    [SerializeField] private Light playerLight;

    [SerializeField] private float intensityChangeSpeed;
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
}
