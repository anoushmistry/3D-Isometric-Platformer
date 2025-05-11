using UnityEngine;

public class LightOrbInteractable : Interactable
{
    private PlayerInteraction playerInteraction;

    private void Start()
    {
        playerInteraction = FindObjectOfType<PlayerInteraction>();  // Find the PlayerInteraction script
    }

    public override void Interact()
    {
        // Assuming the player can hold the orb
        Debug.Log("Picked up the Light Orb!");

        // Call the PlayerInteraction's PickUpOrb method to pick up the orb
        playerInteraction.PickUpOrb(this.gameObject);
    }
}
