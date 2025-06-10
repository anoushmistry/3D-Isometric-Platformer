using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class LightReceiver : MonoBehaviour
{
    public void Activate()
    {
        Debug.Log("Laser Receiver Activated!");
        // Add your logic here, e.g., open doors, trigger lights, etc.
    }
}
