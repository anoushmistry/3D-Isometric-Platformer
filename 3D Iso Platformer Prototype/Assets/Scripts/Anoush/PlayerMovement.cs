using System.Collections;
using Cinemachine;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

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
    private Vector3 externalMovement;
    [SerializeField] private LayerMask groundLayer;

    [Header("Sphere Cast Settings (For Ground Check)")]
    [SerializeField] float sphereRadius = 0.5f;
    [SerializeField] float castDistance = 0.55f;

    [Header("Movement Settings")]
    [SerializeField] public bool LockInput = false;
    [SerializeField] private float horizontal;
    [SerializeField] private float vertical;

    [Header("Light Emitter Settings")]
    [SerializeField] private LightEmitter currentEmitter;

    [Header("Isometric Movement (Camera Settings)")]
    public float startIsoTransformYValue;
    [SerializeField] private float isoTransformYValue;
    [SerializeField] public CinemachineVirtualCamera cinemachineVirtualCamera;

    [Header("Ladder Settings")]
    public Transform ladderTop;
    public Transform ladderBottom;
    public float climbSpeed = 3f;
    private bool isOnLadder = false;
    private bool isClimbing = false;
    private Transform currentLadder;

    [Header("Respawn Settings")]
    [SerializeField] private Image fadeImageBlack;
    [SerializeField] private float fadeDuration = 0.5f;
    private Vector3 lastGroundedPosition;
    private float lastGroundedSaveTimer = 0f;
    private float saveInterval = 5f;

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
        controller = GetComponent<CharacterController>();
        controller.stepOffset = 0.2f;
        lastGroundedPosition = transform.position;
        animator.stabilizeFeet = true;
    }

    void Update()
    {
        if (cinemachineVirtualCamera != null)
            isoTransformYValue = cinemachineVirtualCamera.transform.eulerAngles.y;

        if (!isOnLadder)
        {
            CalculateInput();
            HandleMovement();
        }
        else
        {
            HandleLadderClimbInput();
        }

        if (isGrounded)
        {
            lastGroundedSaveTimer += Time.deltaTime;
            if (lastGroundedSaveTimer >= saveInterval)
            {
                lastGroundedPosition = transform.position;
                lastGroundedSaveTimer = 0f;
            }
        }
    }

    void CalculateInput()
    {
        if (!LockInput)
        {
            horizontal = Input.GetAxisRaw("Horizontal");
            vertical = Input.GetAxisRaw("Vertical");
        }
        if (LockInput)
        {
            animator.SetFloat("Speed", 0f);
        }
    }

    void HandleMovement()
    {
        if (LockInput)
        {
            return;
        }
        GroundCheck();
        Vector3 inputDirection = new Vector3(horizontal, 0, vertical).normalized;
        Vector3 isoMoveDirection = inputDirection.ToIsometric(isoTransformYValue);

        animator?.SetFloat("Speed", isoMoveDirection.magnitude);

        if (controller.isGrounded && velocity.y < 0)
            velocity.y = -2f;
        else
            velocity.y += gravity * mass * Time.deltaTime;

        if (isGrounded && Input.GetKeyDown(KeyCode.Space))
            velocity.y += Mathf.Sqrt(2 * jumpHeight * -gravity) * mass;

        Vector3 movement = isoMoveDirection * speed * Time.deltaTime;
        if (controller.enabled)
        {
            controller.Move(movement + velocity * Time.deltaTime + externalMovement);
        }
        externalMovement = Vector3.zero;

        if (isoMoveDirection.magnitude >= 0.1f)
        {
            float targetAngle = Mathf.Atan2(isoMoveDirection.x, isoMoveDirection.z) * Mathf.Rad2Deg;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref currentVelocity, rotationSmoothTime);
            transform.rotation = Quaternion.Euler(0, angle, 0);
        }

        bool isWalking = isoMoveDirection.magnitude > 0.1f && isGrounded;

        if (SoundManager.Instance != null)
        {
            if (isWalking)
            {
                SoundManager.Instance.PlayFootstepLoop();
            }
            else
            {
                SoundManager.Instance.StopFootstepLoop();
            }

        }
    }

    void GroundCheck()
    {
        isGrounded = controller.isGrounded;
    }

    public void StickToLadder(Transform ladder)
    {
        isOnLadder = true;
        isClimbing = true;
        LockInput = true;
        velocity = Vector3.zero;

        controller.enabled = false;
        Vector3 snapPosition = ladder.position + (-ladder.forward * -0.75f) + (-ladder.right * 0.75f);
        snapPosition.y = transform.position.y;
        transform.position = snapPosition;
        transform.rotation = Quaternion.LookRotation(-ladder.forward);
        controller.enabled = true;

        currentLadder = ladder;
        animator?.SetBool("isClimbing", true);
        animator?.SetInteger("ClimbingSpeed", 0);
    }

    void HandleLadderClimbInput()
    {
        float verticalInput = Input.GetAxisRaw("Vertical");
        Vector3 climbDirection = new Vector3(0, verticalInput * climbSpeed, 0);
        controller.Move(climbDirection * Time.deltaTime);

        animator?.SetInteger("ClimbingSpeed", (int)verticalInput);

        bool isClimbingNow = Mathf.Abs(verticalInput) > 0.1f;

        if (isClimbingNow)
        {
            SoundManager.Instance.PlayClimbLoop();
        }
        else
        {
            SoundManager.Instance.StopClimbLoop();
        }

        if (transform.position.y >= ladderTop.position.y - 0.5f && verticalInput > 0)
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

        SoundManager.Instance.StopClimbLoop();
        SoundManager.Instance.StopFootstepLoop();

        velocity.y = -1f;
    }

    public void ApplyExternalMovement(Vector3 movement)
    {
        externalMovement += movement;
    }

    public float getIsometricYValue() => isoTransformYValue;

    public void RespawnPlayer()
    {
        StartCoroutine(RespawnPlayerCoroutine());
    }

    private IEnumerator RespawnPlayerCoroutine()
    {
        LockInput = true;

        fadeImageBlack.gameObject.SetActive(true);

        Color fadeColor = fadeImageBlack.color;

        yield return DOTween.To(() => fadeColor.a, x => { fadeColor.a = x; fadeImageBlack.color = fadeColor; }, 1, fadeDuration).SetUpdate(true).WaitForCompletion();

        controller.enabled = false;
        transform.position = lastGroundedPosition + Vector3.up * 1f;
        velocity = Vector3.zero;

        yield return new WaitForSeconds(0.2f);

        yield return DOTween.To(() => fadeColor.a, x => { fadeColor.a = x; fadeImageBlack.color = fadeColor; }, 0, fadeDuration).SetUpdate(true).WaitForCompletion();

        fadeImageBlack.gameObject.SetActive(false);

        controller.enabled = true;
        LockInput = false;
    }
}

public static class HelpersCharacterController
{
    public static Vector3 ToIsometric(this Vector3 input, float yValue)
    {
        Matrix4x4 isoMatrix = Matrix4x4.Rotate(Quaternion.Euler(0, yValue, 0));
        return isoMatrix.MultiplyPoint3x4(input);
    }
}