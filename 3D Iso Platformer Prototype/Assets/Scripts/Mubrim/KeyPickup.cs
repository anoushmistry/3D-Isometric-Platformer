using UnityEngine;

public class KeyPickup : Interactable
{
    public override void Interact()
    {
        HidePrompt();
        PlayerInventory.Instance.PickupKey();
        KeyUIController.Instance.ShowKeyIcon();
        Destroy(gameObject); 
    }
}
