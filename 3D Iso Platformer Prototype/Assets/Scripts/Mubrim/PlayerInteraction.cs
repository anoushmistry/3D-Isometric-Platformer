using UnityEngine;

/// <summary>
/// Handles player interactions: picking/placing orbs, detecting interactables, 
/// rotating mirrors, reading notes, and managing movement locking.
/// </summary>
public class PlayerInteraction : MonoBehaviour
{
    [Header("Interaction Settings")]
    [SerializeField] private float interactionRange = 3f;
    [SerializeField] private float moveSpeed = 5f;

    [Header("Orb Settings")]
    [SerializeField] private Transform orbHolder;

    [Header("Note UI")]
    [SerializeField] private GameObject noteUIPanel;
    [SerializeField] private TMPro.TextMeshProUGUI noteTextUI;

    [Header("References")]
    [SerializeField] private PlayerMovement playerMovement;
    public FloatingBridge bridgePopper;

    [SerializeField] private Interactable nearbyInteractable;
    private OrbPickupHandler heldOrb;

    private bool isHoldingOrb = false;
    private bool orbPlaced = false;
    private bool isLerpingToHolder = false;
    private bool isNoteOpen = false;
    private bool isInMirrorRotationMode = false;

    private Vector3 velocity = Vector3.zero;
    private Vector3 lastPosition;
    private LightMirror currentMirror;

    private float mirrorRotationCooldown = 0f;
    private const float enterRotationDelay = 0.2f;

    private void Awake()
    {
        if (playerMovement == null)
            playerMovement = GetComponent<PlayerMovement>();
    }

    private void Update()
    {
        if (isInMirrorRotationMode)
        {
            HandleMirrorRotationMode();
            return;
        }

        CheckForInteractable();

        if (Input.GetKeyDown(KeyCode.E))
            HandleInteractionInput();

        if (isLerpingToHolder && heldOrb != null)
            LerpOrbToHolder();

        if (isHoldingOrb && heldOrb != null && !isLerpingToHolder)
            heldOrb.transform.localRotation = Quaternion.identity;

        lastPosition = transform.position;
    }

    private void HandleInteractionInput()
    {
        if (isNoteOpen)
        {
            CloseNoteUI();
            return;
        }

        if (nearbyInteractable == null) return;

        if (nearbyInteractable.TryGetComponent(out Note note))
        {
            OpenNoteUI(note.noteText);
        }
        else
        {
            nearbyInteractable.Interact();
        }
    }

    private void HandleMirrorRotationMode()
    {
        mirrorRotationCooldown -= Time.deltaTime;

        bool rotating = false;

        if (mirrorRotationCooldown <= 0f && currentMirror != null)
        {
            float rotationDir = 0f;
            if (Input.GetKey(KeyCode.Q)) rotationDir = -1f;
            if (Input.GetKey(KeyCode.R)) rotationDir = 1f;

            rotating = rotationDir != 0f;

            if (rotationDir != 0f)
                currentMirror.RotateMirror(rotationDir);
        }

        if (rotating)
            SoundManager.Instance?.PlayMirrorRotateLoop();
        else
            SoundManager.Instance?.StopMirrorRotateLoop();

        if (Input.GetKeyDown(KeyCode.E))
            ExitMirrorRotationMode();
    }

    private void CheckForInteractable()
    {
        if (nearbyInteractable != null)
        {
            nearbyInteractable.HidePrompt();
            nearbyInteractable = null;
        }

        Collider[] colliders = Physics.OverlapSphere(transform.position + Vector3.up, interactionRange);
        foreach (var collider in colliders)
        {
            if (collider.TryGetComponent(out Interactable interactable) && interactable.IsInteractable())
            {
                nearbyInteractable = interactable;
                if (interactable.CompareTag("Ladder"))
                {
                    Vector3 camForward = Camera.main.transform.forward;
                    Vector3 offsetPosition = interactable.transform.position + (camForward.normalized * -2f);
                    interactable.ShowPrompt(offsetPosition); 
                }
                else
                {
                    nearbyInteractable.ShowPrompt();
                }
                break;
            }
        }
    }

    private void LerpOrbToHolder()
    {
        heldOrb.transform.position = Vector3.SmoothDamp(
            heldOrb.transform.position,
            orbHolder.position,
            ref velocity,
            0.15f
        );

        if (Vector3.Distance(heldOrb.transform.position, orbHolder.position) < 0.05f)
        {
            heldOrb.transform.position = orbHolder.position;
            heldOrb.transform.SetParent(orbHolder);
            isLerpingToHolder = false;

            if (playerMovement != null)
                playerMovement.enabled = true;
        }
    }

    public void PickUpOrb(Transform orbTransform)
    {
        if (isHoldingOrb) return;

        heldOrb = orbTransform.GetComponent<OrbPickupHandler>();
        if (heldOrb == null)
        {
            Debug.LogWarning("Tried to pick up orb without OrbPickupHandler.");
            return;
        }

        heldOrb.transform.SetParent(null);
        isHoldingOrb = true;
        isLerpingToHolder = true;

        if (playerMovement != null)
            playerMovement.enabled = false;

        SoundManager.Instance?.PlayOrbPickupSFX();
        Debug.Log("Picked up Light Orb!");
    }

    public void PlaceOrb(Vector3 destination)
    {
        if (!isHoldingOrb || heldOrb == null) return;

        heldOrb.transform.SetParent(null);
        heldOrb.MoveToDestination(destination, moveSpeed);

        isHoldingOrb = false;
        orbPlaced = true;
        heldOrb = null;

        if (bridgePopper != null)
            bridgePopper.ActivateBridge();

        SoundManager.Instance?.PlayOrbPlaceSFX();
        Debug.Log("Placed Light Orb!");
    }

    public void EnterMirrorRotationMode(LightMirror mirror)
    {
        currentMirror = mirror;
        isInMirrorRotationMode = true;
        mirrorRotationCooldown = enterRotationDelay;

        if (playerMovement != null)
            playerMovement.enabled = false;
        Animator anim = playerMovement.GetComponentInChildren<Animator>();
        if (anim != null)
            anim.SetFloat("Speed", 0f); 

        Debug.Log("Entered Mirror Rotation Mode");
    }

    public void ExitMirrorRotationMode()
    {
        isInMirrorRotationMode = false;
        currentMirror.EndMirrorInteraction();
        currentMirror = null;

        if (playerMovement != null)
            playerMovement.enabled = true;

        Debug.Log("Exited Mirror Rotation Mode");
    }

    private void OpenNoteUI(string text)
    {
        if (noteUIPanel == null || noteTextUI == null) return;

        noteUIPanel.SetActive(true);
        noteUIPanel.transform.localScale = Vector3.one;
        noteTextUI.text = text;
        isNoteOpen = true;

        if (playerMovement != null)
            playerMovement.LockInput = true;
    }

    private void CloseNoteUI()
    {
        if (noteUIPanel == null || noteTextUI == null) return;

        noteUIPanel.SetActive(false);
        noteUIPanel.transform.localScale = Vector3.zero;
        noteTextUI.text = "";
        isNoteOpen = false;

        if (playerMovement != null)
            playerMovement.LockInput = false;
    }

    public bool IsHoldingOrb() => isHoldingOrb;
    public bool HasPlacedOrb() => orbPlaced;

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position + Vector3.up, interactionRange);
    }
}
