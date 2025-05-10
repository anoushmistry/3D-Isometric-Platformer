using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PushableBox : MonoBehaviour
{
    private Transform player;
    private bool isBeingPushed = false;
    private Rigidbody rb;

    public float followDistance = 1.2f;
    public float moveSpeed = 5f;
    public float boxRadius = 0.5f;
    public LayerMask collisionMask;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.isKinematic = false; // Enable physics
        rb.constraints = RigidbodyConstraints.FreezeRotation;
    }

    void FixedUpdate()
    {
        if (isBeingPushed && player != null)
        {
            Vector3 targetPos = player.position + player.forward * followDistance;
            targetPos.y = transform.position.y;

            // Check if a wall or obstacle is between current box and target position
            Vector3 direction = targetPos - transform.position;
            float distance = direction.magnitude;
            direction.Normalize();

            // Cast to see if there's space to move
            if (!Physics.SphereCast(transform.position, boxRadius, direction, out RaycastHit hit, distance, collisionMask))
            {
                Vector3 newPosition = Vector3.MoveTowards(rb.position, targetPos, moveSpeed * Time.fixedDeltaTime);
                rb.MovePosition(newPosition);
            }
        }
    }

    public void StartPush(Transform playerTransform)
    {
        player = playerTransform;
        isBeingPushed = true;
    }

    public void StopPush()
    {
        isBeingPushed = false;
        player = null;
    }
}