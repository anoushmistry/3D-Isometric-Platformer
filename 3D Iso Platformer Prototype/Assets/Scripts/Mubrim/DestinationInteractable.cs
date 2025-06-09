using UnityEngine;

public class DestinationInteractable : Interactable
{
    private bool isOrbPlaced = false;

    private PlayerInteraction playerInteraction;

    [SerializeField] private Vector3 offset;
    private void Start()
    {
        playerInteraction = FindObjectOfType<PlayerInteraction>();
    }

    public override void Interact()
    {
        if (isOrbPlaced || playerInteraction == null || !playerInteraction.IsHoldingOrb()) return;

        playerInteraction.PlaceOrb(transform.position + offset);
        isOrbPlaced = true;

        HidePrompt(); // Hide E prompt after placing orb
    }

    public override bool IsInteractable()
    {
        return !isOrbPlaced && playerInteraction != null && playerInteraction.IsHoldingOrb();
    }

    public override void ShowPrompt()
    {
        if (IsInteractable())
        {
            base.ShowPrompt(); // Only show prompt when player is holding orb
        }
    }

    public override void HidePrompt()
    {
        base.HidePrompt(); // Cleanly destroys the prompt
    }
}
