using UnityEngine;

public class LightOrbInteraction : Interactable
{
    private bool isPickedUp = false;

    public override void Interact()
    {
        if (isPickedUp) return;

        PlayerInteraction playerInteraction = FindObjectOfType<PlayerInteraction>();

        if (playerInteraction != null)
        {
            playerInteraction.PickUpOrb(transform);
            Debug.Log("Light Orb Picked Up!");

            isPickedUp = true;

            HidePrompt();

            // Disable further interaction by disabling the collider
            Collider col = GetComponent<Collider>();
            if (col != null) col.enabled = false;
        }
    }

    public override void ShowPrompt()
    {
        if (isPickedUp) return;
        base.ShowPrompt();
    }
}
