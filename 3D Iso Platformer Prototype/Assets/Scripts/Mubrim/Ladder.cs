using UnityEngine;

public class Ladder : Interactable
{
    public Transform topPoint;
    public Transform bottomPoint;

    public override void Interact()
    {
        
    }

    public override bool IsInteractable()
    {
        return true;
    }
}
