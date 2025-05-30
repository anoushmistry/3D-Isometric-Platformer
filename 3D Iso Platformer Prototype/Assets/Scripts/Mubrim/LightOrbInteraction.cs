using UnityEngine;

public class LightOrbInteraction : Interactable
{
    public override void Interact()
    {
        PlayerInteraction playerInteraction = FindObjectOfType<PlayerInteraction>();

        if (playerInteraction != null)
        {
            playerInteraction.PickUpOrb(transform); 
            Debug.Log("Light Orb Picked Up!");
        }
    }
}
