using UnityEngine;

/// <summary>
/// Handles interaction logic specific to Light Orbs.
/// Allows the player to pick up the orb once and disables further interaction.
/// </summary>
public class LightOrbInteraction : Interactable
{
    private bool isPickedUp = false;

    public override void Interact()
    {
        // If already picked up, prevent further interaction
        if (isPickedUp)
            return;

        PlayerInteraction playerInteraction = FindObjectOfType<PlayerInteraction>();
        if (playerInteraction != null)
        {
            playerInteraction.PickUpOrb(transform);
            Debug.Log("Light Orb Picked Up!");

            isPickedUp = true;

            HidePrompt();

            Collider orbCollider = GetComponent<Collider>();
            if (orbCollider != null)
                orbCollider.enabled = false;
        }
    }

    public override void ShowPrompt()
    {
        if (!isPickedUp)
            base.ShowPrompt();
    }
}
