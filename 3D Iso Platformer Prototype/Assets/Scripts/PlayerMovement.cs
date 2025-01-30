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

    void Update()
    {
        // Check if the player is grounded
        isGrounded = controller.isGrounded;

        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f; // Small downward force to keep the player grounded
        }

        // Get input axes
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");

        // Get camera's forward and right vectors
        Vector3 camForward = Camera.main.transform.forward;
        Vector3 camRight = Camera.main.transform.right;

        // Ignore Y component (keep movement flat)
        camForward.y = 0;
        camRight.y = 0;

        Debug.Log(Camera.main.transform.forward);
        Debug.Log(Camera.main.transform.right);

        // Normalize directions
        camForward.Normalize();
        camRight.Normalize();

        // Calculate movement direction relative to camera
        moveDirection = (camForward * vertical + camRight * horizontal).normalized;

        // Apply gravity
        velocity.y += gravity * 1.5f * Time.deltaTime;

        // Jumping
        if (isGrounded && Input.GetButtonDown("Jump"))
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }

        // Move the character vertically
        controller.Move(velocity * Time.deltaTime);

        // Only rotate if there is movement
        if (moveDirection.magnitude >= 0.1f)
        {
            // Calculate the target angle based on the movement direction
            float targetAngle = Mathf.Atan2(moveDirection.x, moveDirection.z) * Mathf.Rad2Deg;

            

            // Smoothly interpolate between current and target rotation
            int angle =(int)(Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref currentVelocity, rotationSmoothTime));
            Debug.Log(angle);

            // Apply the smooth rotation
            transform.rotation = Quaternion.Euler(0, angle, 0);

            // Move the player
            controller.Move(moveDirection * speed * Time.deltaTime);
        }
    }
}
