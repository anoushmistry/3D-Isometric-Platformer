using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float speed = 5f;
    public CharacterController controller;
    public float gravity = -9.81f;
    public float jumpHeight = 2f;
    public float rotationSmoothTime = 0.1f;

    private Vector3 moveDirection;
    private Vector3 velocity;
    private float currentVelocity;
    private bool isGrounded;

    public float interactionRange = 3f; // Range for interaction

    void Update()
    {
        // Check if the player is grounded
        isGrounded = controller.isGrounded;
        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }

        // Get input axes
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");

        // Get camera's forward and right vectors
        Vector3 camForward = Camera.main.transform.forward;
        Vector3 camRight = Camera.main.transform.right;

        camForward.y = 0;
        camRight.y = 0;

        camForward.Normalize();
        camRight.Normalize();

        moveDirection = (camForward * vertical + camRight * horizontal).normalized;

        velocity.y += gravity * 1.5f * Time.deltaTime;

        if (isGrounded && Input.GetButtonDown("Jump"))
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }

        controller.Move(velocity * Time.deltaTime);

        if (moveDirection.magnitude >= 0.1f)
        {
            float targetAngle = Mathf.Atan2(moveDirection.x, moveDirection.z) * Mathf.Rad2Deg;
            int angle = (int)(Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref currentVelocity, rotationSmoothTime));
            transform.rotation = Quaternion.Euler(0, angle, 0);
            controller.Move(moveDirection * speed * Time.deltaTime);
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            Interact();
        }
    }

    void Interact()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, 2f); // Check objects in range
        foreach (Collider col in colliders)
        {
            if (col.TryGetComponent(out LightSource lightSource))
            {
                float distance = Vector3.Distance(transform.position, col.transform.position);
                if (distance <= 1.5f) // Set proper range
                {
                    lightSource.ToggleLight();
                    Debug.Log("Light toggled at distance: " + distance);
                    return; // Exit loop after successful interaction
                }
            }
            else if (col.TryGetComponent(out Mirror mirror))
            {
                float distance = Vector3.Distance(transform.position, col.transform.position);
                if (distance <= 1.5f) // Set proper range
                {
                    mirror.RotateMirror();
                    Debug.Log("Mirror rotated at distance: " + distance);
                    return; // Exit loop after successful interaction
                }
            }
        }
    }



}
