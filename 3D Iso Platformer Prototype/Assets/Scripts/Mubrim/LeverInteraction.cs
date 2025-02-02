using UnityEngine;

public class LeverInteraction : MonoBehaviour
{
    public Animator doorAnimator;  // Reference to the door's Animator
    private bool isPlayerInRange = false;  // To check if the player is within range

    private void Start()
    {
        isPlayerInRange = false; // Ensure the player is not in range at the start
    }

    private void Update()
    {
        // Only trigger the door animation when player is in range and presses 'E'
        if (isPlayerInRange && Input.GetKeyDown(KeyCode.E))
        {
            // Set the trigger to activate the door animation
            doorAnimator.SetTrigger("OpenDoor");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // Player has entered the interaction range
            isPlayerInRange = true;
            Debug.Log("Player is in range.");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // Player has moved out of range
            isPlayerInRange = false;
            Debug.Log("Player is out of range.");
        }
    }
}
