using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    [SerializeField] private float interactionRange = 3f;
    private Interactable nearbyInteractable;

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

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, interactionRange);
    }
}
