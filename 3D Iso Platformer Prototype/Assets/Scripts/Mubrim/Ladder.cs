using UnityEngine;

public class Ladder : Interactable
{
    public override void Interact()
    {
        PlayerMovement player = FindObjectOfType<PlayerMovement>();
        if (player != null)
        {
            player.StickToLadder(transform);
        }
        HidePrompt();
    }
}
