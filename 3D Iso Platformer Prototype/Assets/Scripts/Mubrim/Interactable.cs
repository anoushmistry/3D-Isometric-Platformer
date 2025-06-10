using UnityEngine;

/// <summary>
/// Abstract base class for all interactable objects in the game.
/// Handles interaction prompts and camera-facing logic.
/// </summary>
public abstract class Interactable : MonoBehaviour
{
    [Header("Interaction Settings")]
    [Tooltip("Text displayed when the player is near and can interact.")]
    public string promptMessage = "Press E to interact";

    [Tooltip("Prefab shown as a visual prompt for interaction.")]
    public GameObject interactionPromptPrefab;
    private GameObject currentPromptInstance;

    public abstract void Interact();

    public virtual void ShowPrompt()
    {
        if (interactionPromptPrefab == null || currentPromptInstance != null)
            return;

        // Position the prompt above the object using its collider bounds
        Vector3 promptPosition = GetComponent<Collider>().bounds.center + Vector3.up * 3f;
        currentPromptInstance = Instantiate(interactionPromptPrefab, promptPosition, Quaternion.identity);
    }

    public virtual void HidePrompt()
    {
        if (currentPromptInstance != null)
        {
            Destroy(currentPromptInstance);
            currentPromptInstance = null;
        }
    }

    public virtual bool IsInteractable()
    {
        return true;
    }

    protected virtual void Update()
    {
        if (currentPromptInstance != null && Camera.main != null)
        {
            currentPromptInstance.transform.LookAt(Camera.main.transform);
            currentPromptInstance.transform.Rotate(0, 180f, 0); //ensuring the front of the prompt facing camera
        }
    }
}
