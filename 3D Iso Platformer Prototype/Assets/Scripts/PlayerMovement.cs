//using UnityEngine;

//public class PlayerMovement : MonoBehaviour
//{
//    public float speed = 5f;
//    public CharacterController controller;
//    public float gravity = -9.81f;
//    public float jumpHeight = 2f;
//    public float rotationSmoothTime = 0.1f;

//    private Vector3 moveDirection;
//    private Vector3 velocity;
//    private float currentVelocity;
//    private bool isGrounded;

//    void Update()
//    {
//        // Check if the player is grounded
//        isGrounded = controller.isGrounded;

//        if (isGrounded && velocity.y < 0)
//        {
//            velocity.y = -2f; // Small downward force to keep the player grounded
//        }

//        // Get input axes
//        float horizontal = Input.GetAxisRaw("Horizontal");
//        float vertical = Input.GetAxisRaw("Vertical");

//        // Get camera's forward and right vectors
//        Vector3 camForward = Camera.main.transform.forward;
//        Vector3 camRight = Camera.main.transform.right;

//        // Ignore Y component (keep movement flat)
//        camForward.y = 0;
//        camRight.y = 0;

//        Debug.Log(Camera.main.transform.forward);
//        Debug.Log(Camera.main.transform.right);

//        // Normalize directions
//        camForward.Normalize();
//        camRight.Normalize();

//        // Calculate movement direction relative to camera
//        moveDirection = (camForward * vertical + camRight * horizontal).normalized;

//        // Apply gravity
//        velocity.y += gravity * 1.5f * Time.deltaTime;

//        // Jumping
//        if (isGrounded && Input.GetButtonDown("Jump"))
//        {
//            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
//        }

//        // Move the character vertically
//        controller.Move(velocity * Time.deltaTime);

//        // Only rotate if there is movement
//        if (moveDirection.magnitude >= 0.1f)
//        {
//            // Calculate the target angle based on the movement direction
//            float targetAngle = Mathf.Atan2(moveDirection.x, moveDirection.z) * Mathf.Rad2Deg;



//            // Smoothly interpolate between current and target rotation
//            int angle =(int)(Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref currentVelocity, rotationSmoothTime));
//            Debug.Log(angle);

//            // Apply the smooth rotation
//            transform.rotation = Quaternion.Euler(0, angle, 0);

//            // Move the player
//            controller.Move(moveDirection * speed * Time.deltaTime);
//        }
//    }
//}

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

        // Create movement input
        Vector3 inputDirection = new Vector3(horizontal, 0, vertical);

        // Convert to isometric space
        Vector3 isoMoveDirection = inputDirection.ToIsometric().normalized;

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
        if (isoMoveDirection.magnitude >= 0.1f)
        {
            // Calculate the target angle based on the isometric movement direction
            float targetAngle = Mathf.Atan2(isoMoveDirection.x, isoMoveDirection.z) * Mathf.Rad2Deg;

            // Smoothly interpolate between current and target rotation
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref currentVelocity, rotationSmoothTime);

            // Apply the smooth rotation
            transform.rotation = Quaternion.Euler(0, angle, 0);

            // Move the player in the isometric direction
            controller.Move(isoMoveDirection * speed * Time.deltaTime);
        }
    }
}

// Helper class for isometric transformation
public static class HelpersCharacterController
{
    private static Matrix4x4 _isoMatrix = Matrix4x4.Rotate(Quaternion.Euler(0, 45, 0));
    public static Vector3 ToIsometric(this Vector3 input) => _isoMatrix.MultiplyPoint3x4(input);
}

