/*Handles player interactions including picking up/placing orbs, detecting interactables, 
entering/exiting mirror rotation mode, and locking movement during interactions.*/

using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    [SerializeField] private float interactionRange = 3f;
    [SerializeField] private Transform orbHolder;
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private PlayerMovement playerMovement;
    public FloatingBridge bridgePopper;
    [SerializeField] private GameObject noteUIPanel;
    [SerializeField] private TMPro.TextMeshProUGUI noteTextUI;
    private bool isNoteOpen = false;

    private Interactable nearbyInteractable;
    private bool isHoldingOrb = false;
    private OrbPickupHandler heldOrb;
    private bool orbPlaced = false;
    private bool isLerpingToHolder = false;

    private LightMirror currentMirror = null;
    private bool isInMirrorRotationMode = false;
    private Vector3 lastPosition;

    private float mirrorRotationCooldown = 0f;
    private const float enterRotationDelay = 0.2f;

    [SerializeField] private GameObject interactPromptPrefab;
    private GameObject currentPrompt;

    private void Awake()
    {
        if (playerMovement == null)
            playerMovement = GetComponent<PlayerMovement>();
    }

    private Vector3 velocity = Vector3.zero;

    private void Update()
    {
        if (isInMirrorRotationMode)
        {
            mirrorRotationCooldown -= Time.deltaTime;

            if (mirrorRotationCooldown <= 0f && currentMirror != null)
            {
                float rotationDir = 0f;
                if (Input.GetKey(KeyCode.Q)) rotationDir = -1f;
                if (Input.GetKey(KeyCode.R)) rotationDir = 1f;

                if (rotationDir != 0f)
                {
                    currentMirror.RotateMirror(rotationDir);
                }
            }

            if (Input.GetKeyDown(KeyCode.E))
            {
                ExitMirrorRotationMode();
            }

            return;
        }

        CheckForInteractable();
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (isNoteOpen)
            {
                CloseNoteUI();
                return;
            }

            if (nearbyInteractable != null)
            {
                if (currentPrompt != null)
                {
                    Destroy(currentPrompt);
                    currentPrompt = null;
                }

                Note note = nearbyInteractable.GetComponent<Note>();
                if (note != null)
                {
                    OpenNoteUI(note.noteText);
                }
                else
                {
                    nearbyInteractable.Interact();
                }
            }
        }
        if (isLerpingToHolder && heldOrb != null)
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

        if (isHoldingOrb && heldOrb != null && !isLerpingToHolder)
        {
            heldOrb.transform.localRotation = Quaternion.identity; // to prevent spinning
        }

        lastPosition = transform.position;
    }

    private void CheckForInteractable()
    {
        nearbyInteractable = null;

        Collider[] colliders = Physics.OverlapSphere(transform.position + new Vector3(0f, 1f, 0f), interactionRange);
        foreach (var collider in colliders)
        {
            Interactable interactable = collider.GetComponent<Interactable>();
            if (interactable && interactable.IsInteractable())
            {
                nearbyInteractable = interactable;

                if (currentPrompt == null && interactPromptPrefab != null)
                {
                    currentPrompt = Instantiate(interactPromptPrefab);
                }

                if (currentPrompt != null)
                {
                    currentPrompt.SetActive(true);
                    currentPrompt.transform.position = collider.transform.position + Vector3.up * 2f;
                    currentPrompt.transform.forward = Camera.main.transform.forward;
                }

                return;
            }
        }

        if (currentPrompt != null)
        {
            currentPrompt.SetActive(false);
        }
    }



    public void PickUpOrb(Transform orbTransform)
    {
        if (!isHoldingOrb)
        {
            heldOrb = orbTransform.GetComponent<OrbPickupHandler>();
            if (heldOrb != null)
            {
                heldOrb.transform.SetParent(null);
                isHoldingOrb = true;
                isLerpingToHolder = true;

                if (playerMovement != null)
                    playerMovement.enabled = false;

                Debug.Log("Picked up Light Orb!");
            }
            else
            {
                Debug.LogWarning("Orb does not have OrbPickupHandler!");
            }
        }
    }
    public bool IsHoldingOrb()
    {
        return isHoldingOrb;
    }

    public void PlaceOrb(Vector3 destination)
    {
        if (isHoldingOrb && heldOrb != null)
        {
            heldOrb.transform.SetParent(null);
            heldOrb.MoveToDestination(destination, moveSpeed);
            isHoldingOrb = false;
            orbPlaced = true;
            heldOrb = null;
            Debug.Log("Placed Light Orb at destination!");

            if (bridgePopper != null)
                bridgePopper.ActivateBridge();
        }
    }


    public void EnterMirrorRotationMode(LightMirror mirror)
    {
        currentMirror = mirror;
        isInMirrorRotationMode = true;
        lastPosition = transform.position;
        mirrorRotationCooldown = enterRotationDelay;

        if (playerMovement != null)
            playerMovement.enabled = false;

        Debug.Log("Entered Mirror Rotation Mode");
    }

    public void ExitMirrorRotationMode()
    {
        isInMirrorRotationMode = false;
        currentMirror = null;

        if (playerMovement != null)
            playerMovement.enabled = true;

        Debug.Log("Exited Mirror Rotation Mode");
    }

    public bool HasPlacedOrb()
    {
        return orbPlaced;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position + new Vector3(0f, 1f, 0f), interactionRange);
    }

    void OpenNoteUI(string text)
    {
        if (noteUIPanel != null && noteTextUI != null)
        {
            noteUIPanel.SetActive(true);
            noteUIPanel.transform.localScale = Vector3.one; 
            noteTextUI.text = text;
            isNoteOpen = true;

            if (playerMovement != null)
                playerMovement.LockInput = true;
        }
    }

    void CloseNoteUI()
    {
        if (noteUIPanel != null && noteTextUI != null)
        {
            noteUIPanel.SetActive(false);
            noteUIPanel.transform.localScale = Vector3.zero; 
            noteTextUI.text = "";
            isNoteOpen = false;

            if (playerMovement != null)
                playerMovement.LockInput = false;
        }
    }

}