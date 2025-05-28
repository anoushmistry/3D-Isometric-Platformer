using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    [SerializeField] private float interactionRange = 3f;
    [SerializeField] private Transform orbHolder;
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float spinSpeed = 90f;

    private Interactable nearbyInteractable;
    private bool isHoldingOrb = false;
    private OrbPickupHandler heldOrb;
    private bool orbPlaced = false;
    private bool isLerpingToHolder = false;

    private void Update()
    {
        CheckForInteractable();

        if (nearbyInteractable != null && Input.GetKeyDown(KeyCode.E))
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
            heldOrb.transform.Rotate(Vector3.up * spinSpeed * Time.deltaTime, Space.Self);
        }
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

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, interactionRange);
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

    public bool HasPlacedOrb()
    {
        return orbPlaced;
    }
}
