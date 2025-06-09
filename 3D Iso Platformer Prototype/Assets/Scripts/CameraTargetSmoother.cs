using UnityEngine;

public class CameraTargetSmoother : MonoBehaviour
{
    public Transform player;
    public float smoothSpeed = 5f;

    private float currentY;

    void LateUpdate()
    {
        Vector3 targetPosition = player.position;

        // Smooth only Y
        currentY = Mathf.Lerp(currentY, targetPosition.y, smoothSpeed * Time.deltaTime);

        transform.position = new Vector3(targetPosition.x, currentY, targetPosition.z);
    }

    void Start()
    {
        currentY = player.position.y;
    }
}