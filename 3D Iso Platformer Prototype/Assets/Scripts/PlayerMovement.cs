using System;
using Cinemachine;
using DG.Tweening;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

    public static PlayerMovement Instance;

    [Header("Player Movement Settings")]
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

    [Header("Sphere Cast Settings (For Ground Check)")]
    [SerializeField] float sphereRadius = 0.5f;
    [SerializeField] float castDistance = 0.55f;

    [Header("Sliding Settings (Currently Not in Use)")]
    [SerializeField] private float slideSpeed;
    [SerializeField] private bool isSliding;
    [SerializeField] private float slopeLimit;
    [SerializeField] private Rigidbody rb;


    [Header("Movement Settings")]
    [SerializeField] public bool LockInput = false;
    [SerializeField] private float horizontal;
    [SerializeField] private float vertical;

    [Header("Interaction")]
    [SerializeField] private float interactionRange = 3f;
    public LayerMask interactableLayer;

    [Header("Light Emitter Settings")]
    [SerializeField] private LightEmitter currentEmitter;

    [Header("Isometric Movement (Camera Settings)")]
    [Tooltip("The start value of the cinemachine camera angle. This is helpful for switching back to the original Camera angle after an angle change")]
    public float startIsoTransformYValue;
    [Tooltip("The Y rotation of the camera in isometric view. Adjust this to match your camera's angle.")]
    [SerializeField] private float isoTransformYValue;
    [Tooltip("Cinemachine Virtual Camera for isometric view")]
    [SerializeField] public CinemachineVirtualCamera cinemachineVirtualCamera;

    [Header("Ladder Settings")]
    public Transform ladderTop;
    public Transform ladderBottom;
    public float climbSpeed = 3f;
    public LayerMask ladderLayer;
    private bool isOnLadder = false;
    private bool isClimbing = false;
    private Transform currentLadder;
    private bool promptShown = false;

    private Interactable currentLadderPrompt;

    private void Awake()
    {
        if (Instance)
        {
            Destroy(Instance);
            Instance = this;
        }
        else
        {
            Instance = this;
        }
        if (cinemachineVirtualCamera != null)
        {
            isoTransformYValue = cinemachineVirtualCamera.transform.eulerAngles.y;
            startIsoTransformYValue = cinemachineVirtualCamera.transform.eulerAngles.y;
        }
    }

    private void Start()
    {
        animator = GetComponentInChildren<Animator>();
        rb = GetComponent<Rigidbody>();
        controller = GetComponent<CharacterController>();
    }

    void Update()
    {
        if (cinemachineVirtualCamera != null)
            isoTransformYValue = cinemachineVirtualCamera.transform.eulerAngles.y;

        if (!isOnLadder)
        {
            CalculateInput();
            HandleMovement();
            //DetectLadderPrompt();
        }
        else
        {
            HandleLadderClimbInput();
        }
    }

    void CalculateInput()
    {
        if (!LockInput)
        {
            horizontal = Input.GetAxisRaw("Horizontal");
            vertical = Input.GetAxisRaw("Vertical");
        }
    }

    void HandleMovement()
    {
        GroundCheck();
        Vector3 inputDirection = new Vector3(horizontal, 0, vertical).normalized;
        Vector3 isoMoveDirection = inputDirection.ToIsometric(isoTransformYValue);

        if (animator != null)
            animator.SetFloat("Speed", isoMoveDirection.magnitude);

        if (controller.isGrounded && velocity.y < 0)
            velocity.y = -2f;
        else
            velocity.y += gravity * mass * Time.deltaTime;

        if (isGrounded && Input.GetKeyDown(KeyCode.Space))
            velocity.y += Mathf.Sqrt(2 * jumpHeight * -gravity) * mass;

        Vector3 movement = isoMoveDirection * speed * Time.deltaTime;
        controller.Move(movement + velocity * Time.deltaTime);

        if (isoMoveDirection.magnitude >= 0.1f)
        {
            float targetAngle = Mathf.Atan2(isoMoveDirection.x, isoMoveDirection.z) * Mathf.Rad2Deg;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref currentVelocity, rotationSmoothTime);
            transform.rotation = Quaternion.Euler(0, angle, 0);
        }
    }

    void GroundCheck()
    {
        isGrounded = controller.isGrounded;
    }

    /*void DetectLadderPrompt()
    {
        Ray ray = new Ray(transform.position + Vector3.up * 0.5f, transform.forward);
        if (Physics.Raycast(ray, out RaycastHit hit, 1f, ladderLayer))
        {
            if (!promptShown && hit.transform.TryGetComponent(out Interactable interactable))
            {
                currentLadder = hit.transform;
                currentLadderPrompt = interactable;
                interactable.ShowPrompt();
                promptShown = true;
            }

            if (promptShown && Input.GetKeyDown(KeyCode.E))
            {
                currentLadderPrompt?.HidePrompt();
                currentLadderPrompt = null;
                promptShown = false;
                StickToLadder(currentLadder);
            }
        }
        else
        {
            if (promptShown)
            {
                currentLadderPrompt?.HidePrompt();
                promptShown = false;
                currentLadderPrompt = null;
            }
        }
    }*/

    public void StickToLadder(Transform ladder)
    {
        isOnLadder = true;
        isClimbing = true;
        LockInput = true;
        velocity = Vector3.zero;

        controller.enabled = false;
        Vector3 snapPosition = ladder.position + (-ladder.forward * -0.5f); // offset to stay in front
        snapPosition.y = transform.position.y;
        //snapPosition.y = ladderBottom.position.y;
        transform.position = snapPosition;
        transform.rotation = Quaternion.LookRotation(-ladder.forward);
        controller.enabled = true;

        currentLadder = ladder;
        animator?.SetBool("isClimbing", true);  // Plays Ladder climb animation
        animator?.SetInteger("ClimbingSpeed", 0);  // Plays Ladder idle animation
    }


    void HandleLadderClimbInput()
    {
        float verticalInput = Input.GetAxisRaw("Vertical");
        Vector3 climbDirection = new Vector3(0, verticalInput * climbSpeed, 0);
        controller.Move(climbDirection * Time.deltaTime);

        //   animator?.SetFloat("Speed", Mathf.Abs(verticalInput));

        animator?.SetInteger("ClimbingSpeed", (int)verticalInput);

        if (transform.position.y >= ladderTop.position.y - 0.1f && verticalInput > 0)
        {
            ExitLadder(new Vector3(transform.position.x, ladderTop.position.y, transform.position.z));
        }
        else if (transform.position.y <= ladderBottom.position.y + 0.1f && verticalInput < 0)
        {
            ExitLadder(new Vector3(transform.position.x, ladderBottom.position.y, transform.position.z));
        }
    }

    void ExitLadder(Vector3 exitPosition)
    {
        animator?.SetBool("isClimbing", false);
        animator?.SetInteger("ClimbingSpeed", 0);
        isOnLadder = false;
        isClimbing = false;
        LockInput = false;

        controller.enabled = false;
        transform.position = exitPosition;
        controller.enabled = true;

        velocity.y = -1f; // reapply slight gravity to trigger grounded

    }

    public float getIsometricYValue() => isoTransformYValue;

    
}

public static class HelpersCharacterController
{
    public static Vector3 ToIsometric(this Vector3 input, float yValue)
    {
        Matrix4x4 isoMatrix = Matrix4x4.Rotate(Quaternion.Euler(0, yValue, 0));
        return isoMatrix.MultiplyPoint3x4(input);
    }
}
