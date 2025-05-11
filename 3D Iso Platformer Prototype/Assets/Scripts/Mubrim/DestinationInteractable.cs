using UnityEngine;

public class DestinationInteractable : Interactable
{
    [SerializeField] private Transform placementLocation;  // Where the orb will be placed

    public override void Interact()
    {
        PlayerInteraction playerInteraction = FindObjectOfType<PlayerInteraction>();
        playerInteraction.PlaceOrb(placementLocation);

        // Set the orbPlaced flag to true
        OrbPlacementManager.SetOrbPlaced();

        Debug.Log("Placed the Light Orb. Door can now be opened!");
    }
}
