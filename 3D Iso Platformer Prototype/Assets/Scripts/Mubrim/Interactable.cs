using UnityEngine;

public abstract class Interactable : MonoBehaviour
{
    [Header("Interaction Settings")]
    public string promptMessage = "Press E to interact";
    public abstract void Interact();
}
