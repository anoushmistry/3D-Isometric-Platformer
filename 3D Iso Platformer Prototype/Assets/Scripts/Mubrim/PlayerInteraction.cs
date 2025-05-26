using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    [SerializeField] private float interactionRange = 3f;
    [SerializeField] private Interactable nearbyInteractable;

    private bool isHoldingOrb = false;
    private Transform heldOrb;

    private bool orbPlaced = false;

    private void Update()
    {
        CheckForInteractable();

        if (nearbyInteractable != null && Input.GetKeyDown(KeyCode.E))
        {
            nearbyInteractable.Interact();
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

    public void PickUpOrb(Transform orb)
    {
        if (!isHoldingOrb)
        {
            heldOrb = orb;
            isHoldingOrb = true;
            orb.gameObject.SetActive(false); // Hide orb when picked
            Debug.Log("Picked up Light Orb!");
        }
    }

    public void PlaceOrb(Vector3 position)
    {
        if (isHoldingOrb)
        {
            heldOrb.position = position;
            heldOrb.gameObject.SetActive(true); // Show orb at destination
            isHoldingOrb = false;
            orbPlaced = true;
            Debug.Log("Placed Light Orb at destination!");
        }
    }

    public bool HasPlacedOrb()
    {
        return orbPlaced;
    }
}
