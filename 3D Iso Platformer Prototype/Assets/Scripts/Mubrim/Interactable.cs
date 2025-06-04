using UnityEngine;

public abstract class Interactable : MonoBehaviour
{
    [Header("Interaction Settings")]
    public string promptMessage = "Press E to interact";
    public abstract void Interact();

    public GameObject interactionPromptPrefab;
    private GameObject currentPromptInstance;

    public virtual void ShowPrompt()
    {
        if (interactionPromptPrefab != null && currentPromptInstance == null)
        {
            currentPromptInstance = Instantiate(interactionPromptPrefab, transform.position + Vector3.up * 2f, Quaternion.identity);
        }
    }

    public virtual void HidePrompt()
    {
        if (currentPromptInstance != null)
        {
            Destroy(currentPromptInstance);
        }
    }
    public virtual bool IsInteractable()
    {
        return true; // By default, all interactables are interactable
    }
    protected virtual void Update()
    {
        if (currentPromptInstance != null)
        {
            currentPromptInstance.transform.LookAt(Camera.main.transform);
            currentPromptInstance.transform.Rotate(0, 180, 0); // face camera
        }
    }
}
