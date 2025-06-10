using UnityEngine;

public class HorizontalPlatform : MonoBehaviour
{
    private Vector3 previousPosition;
    private Transform playerTransform;
    private PlayerMovement playerMovement;

    private void Start()
    {
        previousPosition = transform.position;
    }

    private void Update()
    {
        if (playerMovement != null)
        {
            Vector3 platformDelta = transform.position - previousPosition;
            playerMovement.ApplyExternalMovement(platformDelta);
        }

        previousPosition = transform.position;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerTransform = other.transform;
            playerMovement = other.GetComponent<PlayerMovement>();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (other.transform == playerTransform)
            {
                playerTransform = null;
                playerMovement = null;
            }
        }
    }
}
