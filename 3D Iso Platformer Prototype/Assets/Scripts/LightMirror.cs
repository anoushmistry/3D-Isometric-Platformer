using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class LightMirror : MonoBehaviour
{
    public void ReflectLaser(Vector3 hitPoint, Vector3 incomingDirection, LineRenderer lineRenderer)
    {
        Vector3 normal = transform.forward;
        Vector3 reflectedDirection = Vector3.Reflect(incomingDirection, normal);

        RaycastHit hit;
        if (Physics.Raycast(hitPoint, reflectedDirection, out hit, 50f))
        {
            lineRenderer.positionCount = 3; // Add a new point for reflection
            lineRenderer.SetPosition(2, hit.point);
                
            if (hit.collider.CompareTag("LightReceiver"))
            {
                hit.collider.GetComponent<LightReceiver>().Activate();
            }
        }
    }
}
