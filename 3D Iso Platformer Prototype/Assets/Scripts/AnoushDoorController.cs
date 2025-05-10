using UnityEngine;

public class AnoushDoorController : MonoBehaviour
{
    public Vector3 openOffset = new Vector3(0, 3f, 0);
    public float moveSpeed = 2f;

    private Vector3 closedPosition;
    private Vector3 targetPosition;

    void Start()
    {
        closedPosition = transform.position;
        targetPosition = closedPosition;
    }

    public void OpenDoor()
    {
        targetPosition = closedPosition + openOffset;
    }

    public void CloseDoor()
    {
        targetPosition = closedPosition;
    }

    void Update()
    {
        transform.position = Vector3.Lerp(transform.position, targetPosition, Time.deltaTime * moveSpeed);
    }
}