using UnityEngine;

public class LightBeam : MonoBehaviour
{
    private LineRenderer lineRenderer;
    public int maxReflections = 5; // Max times the beam can reflect
    public float maxDistance = 30f; // How far the beam can go

    void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.positionCount = 1; // Start with just the light source position
    }

    void Update()
    {
        if (!gameObject.activeSelf) return; // Don't update if the beam is off

        // Cast the light beam only when the game object is active and has not been updated before
        CastLight();
    }

    public void CastLight()
    {
        Vector3 startPosition = transform.position;
        Vector3 direction = transform.forward;
        lineRenderer.positionCount = 1;
        lineRenderer.SetPosition(0, startPosition);

        for (int i = 0; i < maxReflections; i++)
        {
            Ray ray = new Ray(startPosition, direction);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, maxDistance))
            {
                lineRenderer.positionCount++;
                lineRenderer.SetPosition(lineRenderer.positionCount - 1, hit.point);

                if (hit.collider.CompareTag("Mirror"))
                {
                    // Reflect the beam based on surface normal
                    direction = Vector3.Reflect(direction, hit.normal);
                    startPosition = hit.point;
                }
                else if (hit.collider.CompareTag("Receiver"))
                {
                    hit.collider.GetComponent<LightReceiver1>().ActivateReceiver();
                    break; // Stop reflecting
                }
                else
                {
                    break; // Hit something that isn't a mirror or receiver
                }
            }
            else
            {
                // If no hit, extend the beam to max distance
                lineRenderer.positionCount++;
                lineRenderer.SetPosition(lineRenderer.positionCount - 1, startPosition + direction * maxDistance);
                break;
            }
        }
    }
}
