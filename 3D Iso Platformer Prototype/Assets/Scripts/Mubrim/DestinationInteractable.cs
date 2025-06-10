using UnityEngine;

/// <summary>
/// Represents a destination point where a Light Orb can be placed.
/// Allows interaction only if the player is holding an orb and no orb has been placed yet.
/// </summary>
public class DestinationInteractable : Interactable
{
    private bool isOrbPlaced = false;

    private PlayerInteraction playerInteraction;

    [SerializeField] private Vector3 offset;
    private void Start()
    {
        playerInteraction = FindObjectOfType<PlayerInteraction>();
    }

    /// <summary>
    /// Called when the player interacts with this pedestal.
    /// If conditions are met, places the orb at this location.
    /// </summary>
    public override void Interact()
    {
        if (isOrbPlaced || playerInteraction == null || !playerInteraction.IsHoldingOrb())
            return;

        playerInteraction.PlaceOrb(transform.position + offset);
        isOrbPlaced = true;

        HidePrompt();
    }

    /// <summary>
    /// Checks whether this pedestal is currently valid for interaction.
    /// Only interactable if the player is holding an orb and none is placed yet.
    /// </summary>
    public override bool IsInteractable()
    {
        return !isOrbPlaced && playerInteraction != null && playerInteraction.IsHoldingOrb();
    }

    /// <summary>
    /// Shows the interaction prompt only if this pedestal is currently interactable.
    /// </summary>
    public override void ShowPrompt()
    {
        if (IsInteractable())
            base.ShowPrompt();
    }
    public override void HidePrompt()
    {
        base.HidePrompt();
    }
}
