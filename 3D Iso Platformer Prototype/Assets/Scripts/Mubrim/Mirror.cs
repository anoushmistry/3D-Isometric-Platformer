using UnityEngine;

public class Mirror : MonoBehaviour
{
    public void RotateMirror()
    {
        transform.Rotate(0, 90, 0);
    }

    public void ReflectLight(Vector3 hitPoint, Vector3 incomingDirection, LightBeam lightBeamPrefab)
    {
        // Calculate reflected direction
        Vector3 normal = transform.forward;
        Vector3 reflectedDirection = Vector3.Reflect(incomingDirection, normal);

        // Instantiate a new LightBeam, NOT a Mirror
        LightBeam newBeam = Instantiate(lightBeamPrefab, hitPoint, Quaternion.LookRotation(reflectedDirection));
        newBeam.CastLight();
    }
}
