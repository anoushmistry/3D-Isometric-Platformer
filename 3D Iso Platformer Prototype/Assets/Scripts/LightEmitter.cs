using UnityEngine;

public class LightEmitter : MonoBehaviour
{
    public float maxDistance = 50f;
    public LayerMask collisionMask;
    public float rotationSpeed = 50f;

    private LineRenderer lineRenderer;
    private bool isInteracting = false;

    void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
    }

    void Update()
    {
        FireLaser();

        if (isInteracting)
        {
            RotateLaser();
        }
    }

    void FireLaser()
    {
        Vector3 startPoint = transform.position;
        Vector3 direction = transform.forward;

        lineRenderer.positionCount = 1;
        lineRenderer.SetPosition(0, startPoint);

        CastLaser(startPoint, direction, 0);
    }

    void CastLaser(Vector3 startPoint, Vector3 direction, int reflections)
    {
        if (reflections > 5) return; // Limit reflections to prevent infinite loops

        RaycastHit hit;
        if (Physics.Raycast(startPoint, direction, out hit, maxDistance, collisionMask))
        {
            lineRenderer.positionCount++;
            lineRenderer.SetPosition(lineRenderer.positionCount - 1, hit.point);

            if (hit.collider.CompareTag("Mirror"))
            {
                Vector3 reflectDirection = Vector3.Reflect(direction, hit.normal);
                CastLaser(hit.point, reflectDirection, reflections + 1);
            }
            else if (hit.collider.CompareTag("LightReceiver"))
            {
                hit.collider.GetComponent<LightReceiver>().Activate();
            }
        }
        else
        {
            lineRenderer.positionCount++;
            lineRenderer.SetPosition(lineRenderer.positionCount - 1, startPoint + direction * maxDistance);
        }
    }

    void RotateLaser()
    {
        if (Input.GetKey(KeyCode.Q))
        {
            transform.Rotate(Vector3.up, -rotationSpeed * Time.deltaTime);
        }
        if (Input.GetKey(KeyCode.E))
        {
            transform.Rotate(Vector3.up, rotationSpeed * Time.deltaTime);
        }
    }

    public void ToggleInteraction()
    {
        isInteracting = !isInteracting;
    }
}
