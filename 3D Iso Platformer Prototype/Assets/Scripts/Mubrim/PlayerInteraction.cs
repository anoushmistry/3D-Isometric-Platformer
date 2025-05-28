using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    [SerializeField] private float interactionRange = 3f;
    [SerializeField] private Transform orbHolder;
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private PlayerMovement playerMovement;
    //[SerializeField] private float spinSpeed = 90f;

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
    private void Awake()
    {
        if (playerMovement == null)
            playerMovement = GetComponent<PlayerMovement>();
    }
    private Vector3 velocity = Vector3.zero;
    private void Update()
    {
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

                // Re-enable movement after pickup
                if (playerMovement != null)
                    playerMovement.enabled = true;
            }
        }
        if (isInMirrorRotationMode && currentMirror != null)
        {
            // Countdown the cooldown timer
            mirrorRotationCooldown -= Time.deltaTime;

            // Handle rotation input only after delay
            if (mirrorRotationCooldown <= 0f)
            {
                float rotationDir = 0f;
                if (Input.GetKey(KeyCode.Q)) rotationDir = -1f;
                if (Input.GetKey(KeyCode.E)) rotationDir = 1f;

                if (rotationDir != 0f)
                    currentMirror.RotateMirror(rotationDir);
            }

            // Exit if player moves
            if (Vector3.Distance(transform.position, lastPosition) > 0.05f)
                ExitMirrorRotationMode();

            return;
        }

        CheckForInteractable();

        if (nearbyInteractable != null && Input.GetKeyUp(KeyCode.E)) 
        {
            nearbyInteractable.Interact();
        }


        if (isLerpingToHolder && heldOrb != null)
        {
            heldOrb.transform.position = Vector3.Lerp(
                heldOrb.transform.position,
                orbHolder.position,
                Time.deltaTime * moveSpeed
            );

            if (Vector3.Distance(heldOrb.transform.position, orbHolder.position) < 0.05f)
            {
                heldOrb.transform.position = orbHolder.position;
                heldOrb.transform.SetParent(orbHolder);
                isLerpingToHolder = false;
            }
        }

        if (isHoldingOrb && heldOrb != null && !isLerpingToHolder)
        {
            heldOrb.transform.Rotate(Vector3.up * Time.deltaTime, Space.Self);
        }

        lastPosition = transform.position;
    }

    private void CheckForInteractable()
    {
        nearbyInteractable = null;

        Collider[] colliders = Physics.OverlapSphere(transform.position, interactionRange);
        foreach (var collider in colliders)
        {
            Interactable interactable = collider.GetComponent<Interactable>();
            if (interactable)
            {
                nearbyInteractable = interactable;
                break;
            }
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

                // Disable movement during lerp
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
        }
    }

    public void EnterMirrorRotationMode(LightMirror mirror)
    {
        currentMirror = mirror;
        isInMirrorRotationMode = true;
        lastPosition = transform.position;
        mirrorRotationCooldown = enterRotationDelay; // Start delay
        Debug.Log("Entered Mirror Rotation Mode");
    }

    public void ExitMirrorRotationMode()
    {
        isInMirrorRotationMode = false;
        currentMirror = null;
        Debug.Log("Exited Mirror Rotation Mode");
    }

    public bool HasPlacedOrb()
    {
        return orbPlaced;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, interactionRange);
    }
}
