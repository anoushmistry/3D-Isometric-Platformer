using UnityEngine;

public class DestinationInteractable : Interactable
{
    public override void Interact()
    {
        PlayerInteraction playerInteraction = FindObjectOfType<PlayerInteraction>();
        if (playerInteraction != null)
        {
            playerInteraction.PlaceOrb(transform.position);
        }
    }
}
