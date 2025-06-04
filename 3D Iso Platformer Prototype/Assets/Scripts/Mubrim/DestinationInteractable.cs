using UnityEngine;
using System.Collections;

public class DestinationInteractable : Interactable
{
    private bool isOrbPlaced = false;

    public float dropDuration = 1.0f; 
    private Vector3 initialPosition;
    private Vector3 targetPosition;

    private void Start()
    {
        initialPosition = transform.position;
        targetPosition = new Vector3(initialPosition.x, 0.5f, initialPosition.z);
    }

    public override void Interact()
    {
        if (isOrbPlaced) return;

        PlayerInteraction playerInteraction = FindObjectOfType<PlayerInteraction>();
        if (playerInteraction != null)
        {
            playerInteraction.PlaceOrb(transform.position);
            isOrbPlaced = true;
            StartCoroutine(LerpDown());
        }
    }

    private IEnumerator LerpDown()
    {
        float elapsed = 0f;
        Vector3 start = transform.position;

        while (elapsed < dropDuration)
        {
            transform.position = Vector3.Lerp(start, targetPosition, elapsed / dropDuration);
            elapsed += Time.deltaTime;
            yield return null;
        }

        transform.position = targetPosition; 
    }
}
