using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    [SerializeField] private float interactionRange = 3f;
    private Interactable nearbyInteractable;
    private GameObject heldOrb;  // Reference to the orb the player is holding

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
                Debug.Log(interactable.promptMessage);
                break;
            }
        }
    }

    public void PickUpOrb(GameObject orb)
    {
        heldOrb = orb;
        orb.SetActive(false);  // Disable the orb to simulate "picking it up"
    }

    public void PlaceOrb(Transform placementLocation)
    {
        if (heldOrb != null)
        {
            heldOrb.SetActive(true);  // Re-enable the orb when placing it
            heldOrb.transform.position = placementLocation.position;
            heldOrb = null;  // Release the orb after placing it
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, interactionRange);
    }
}
