using Unity.Burst.CompilerServices;
using UnityEngine;
using UnityEngine.UIElements;
using static UnityEngine.UI.Image;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float speed = 5f;
    [SerializeField] private CharacterController controller;
    [SerializeField] private float gravity = -9.81f;
    [SerializeField] private float jumpHeight = 2f;
    [SerializeField] private float rotationSmoothTime = 0.1f;
    [SerializeField] private int mass;
    private Vector3 velocity;
    private float currentVelocity;
    private bool isGrounded;
    private Animator animator;
    [SerializeField] private LayerMask groundLayer;

    [SerializeField] float sphereRadius = 0.5f;
    [SerializeField] float castDistance = 0.55f;

    [SerializeField] private float slideSpeed;
    [SerializeField] private bool isSliding;
    [SerializeField] private float slopeLimit;
    [SerializeField] private Rigidbody rb;


    [SerializeField] private bool LockInput = false;
    [SerializeField] private float horizontal;
    [SerializeField] private float vertical;


    [SerializeField] private float interactionRange = 3f; // Distance to interact
    public LayerMask interactableLayer; // Layer for interactable objects
    [SerializeField] private LightEmitter currentEmitter;

    private void Start()
    {
        animator = GetComponentInChildren<Animator>();
        rb = GetComponent<Rigidbody>();
        controller = GetComponent<CharacterController>();
    }

    void CalculateInput()
    {
        if (!LockInput)
        {
            horizontal = Input.GetAxisRaw("Horizontal");
            vertical = Input.GetAxisRaw("Vertical");
        }


    }
    void Update()
    {

        DetectInteractable();

        if (currentEmitter != null && Input.GetKeyDown(KeyCode.F)) // Press 'F' to interact
        {
            currentEmitter.ToggleInteraction();
        }

        CalculateInput();
        // Check if grounded using raycast
        GroundCheck();
        // SlopeCheck();

        // Handle player input for movement

        // Calculate movement direction
        Vector3 inputDirection = new Vector3(horizontal, 0, vertical).normalized;

        // Convert to isometric space
        Vector3 isoMoveDirection = inputDirection.ToIsometric();

        if(animator != null)
        animator.SetFloat("Speed", isoMoveDirection.magnitude);
        // Apply gravity
        //if (!isGrounded)
        //{
        //    velocity.y += gravity * mass * Time.deltaTime;  // Apply gravity when not grounded
        //}
        //else if (velocity.y < 0) 
        //{

        //    velocity.y = -2f;  // Small downward force to keep player grounded
        //}
        if (controller.isGrounded && velocity.y < 0)
        {
            velocity.y = -2f; // Keeps player "stuck" to ground
        }
        else
        {
            velocity.y += gravity * mass * Time.deltaTime;
        }

        // Jump logic
        if (isGrounded && Input.GetKeyDown(KeyCode.Space))
        {
            //velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);  // Jump force
            float jumpForce = Mathf.Sqrt(2 * jumpHeight * -gravity) * mass;
            velocity.y += jumpForce;
        }

        // Calculate final movement
        Vector3 movement = isoMoveDirection * speed * Time.deltaTime;

        // Apply horizontal movement and vertical velocity to controller
        controller.Move(movement + velocity * Time.deltaTime);

        // Handle rotation if there's movement
        if (isoMoveDirection.magnitude >= 0.1f)
        {
            float targetAngle = Mathf.Atan2(isoMoveDirection.x, isoMoveDirection.z) * Mathf.Rad2Deg;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref currentVelocity, rotationSmoothTime);
            transform.rotation = Quaternion.Euler(0, angle, 0);  // Smooth rotation
        }

    }

    private void GroundCheck()
    {
        // Spherecast to check if the player is on the ground
        isGrounded = controller.isGrounded;
        //isGrounded = Physics.SphereCast(transform.position + new Vector3(0,1f,0), sphereRadius, Vector3.down, out RaycastHit hit, castDistance, groundLayer);

        // Debugging the hit result
        if (isGrounded)
            Debug.Log("Ground Detected: ");
        else
            Debug.Log("Not Grounded");
    }
    #region SlopeCheck
    void SlopeCheck()
    {

        float sphereCastoffsetY = controller.height / 2 - controller.radius;
        Vector3 castOrigin = transform.position - new Vector3(0, sphereCastoffsetY, 0);
        if (Physics.SphereCast(castOrigin, sphereRadius - .02f, Vector3.down, out RaycastHit hit, castDistance, groundLayer, QueryTriggerInteraction.Ignore))
        {
            // Get the angle between the surface normal and the up direction
            float angle = Vector3.Angle(hit.normal, Vector3.up);
            Debug.Log($"Surface angle: {angle}");
            Debug.DrawLine(hit.point, hit.point + hit.normal, Color.magenta, 3f);
            // If the surface angle is greater than the slope limit, initiate sliding
            if (angle > slopeLimit)
            {
                if (!isSliding)
                {
                    isSliding = true;
                    Debug.Log("Sliding initiated!");
                }

                LockInput = true;
                Vector3 slideDirection = Vector3.ProjectOnPlane(Vector3.down, hit.normal).normalized;
                velocity = slideDirection * slideSpeed;
            }
            else
            {
                if (isSliding)
                {
                    LockInput = false;
                    isSliding = false;
                    Debug.Log("Sliding stopped!");
                    velocity = new Vector3(0, 0, 0);
                }

                // Reset velocity to zero when the slope is walkable
                // velocity = Vector3.zero;
            }
        }

        // Apply the velocity to move the character
        controller.Move(velocity * Time.deltaTime);
    }
    #endregion

    void DetectInteractable()
    {
        Ray ray = new Ray(transform.position + new Vector3(0,1.5f,0) /* the pivot is the foot that's why*/, transform.forward);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, interactionRange, interactableLayer))
        {
            if (hit.collider.GetComponentInChildren<LightEmitter>())
            {
                currentEmitter = hit.collider.GetComponentInChildren<LightEmitter>();
                Debug.Log("Press 'F' to interact with Laser Emitter.");
            }
        }
        else
        {
            currentEmitter = null;
        }
    }
    private void OnDrawGizmos()
    {
        Vector3 origin = transform.position + new Vector3(0,1f,0);
        Vector3 direction = Vector3.down;
        Vector3 endPoint = origin + direction * castDistance;

        // Color based on detection
        Gizmos.color = isGrounded ? Color.green : Color.red;

        // Draw the line representing the cast direction
        Gizmos.DrawLine(origin, endPoint);

        // Draw the starting sphere
        Gizmos.DrawWireSphere(origin, sphereRadius);

        // Perform a SphereCast to update hit.point in real-time
        if (Physics.SphereCast(origin, sphereRadius, direction, out RaycastHit debugHit, castDistance, groundLayer))
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(debugHit.point, sphereRadius);
        }
        else
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(endPoint, sphereRadius);
        }
    }
    void OnControllerColliderHit(ControllerColliderHit hit)
    {
        Rigidbody body = hit.collider.attachedRigidbody;

        // no rigidbody
        if (body == null || body.isKinematic)
        {
            return;
        }

        // We dont want to push objects below us
        if (hit.moveDirection.y < -0.3)
        {
            return;
        }

        // Calculate push direction from move direction,
        // we only push objects to the sides never up and down
        Vector3 pushDir = new Vector3(hit.moveDirection.x, 0, hit.moveDirection.z);

        // If you know how fast your character is trying to move,
        // then you can also multiply the push velocity by that.

        // Apply the push
        body.linearVelocity = pushDir * 5;
    }
}

// Helper class for isometric transformation
public static class HelpersCharacterController
{
    private static Matrix4x4 isoMatrix = Matrix4x4.Rotate(Quaternion.Euler(0, 45, 0));

    public static Vector3 ToIsometric(this Vector3 input)
    {
        return isoMatrix.MultiplyPoint3x4(input);


        // Apply the isometric transformation to the X and Z axes while keeping Y unchanged
        Vector3 iso = isoMatrix.MultiplyPoint3x4(new Vector3(input.x, 0, input.z)); // Only transform X and Z
        iso.y = input.y; // Keep the original Y value unchanged
        return iso;
    }
}

