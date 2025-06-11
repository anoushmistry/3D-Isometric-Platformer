using UnityEngine;

/// <summary>
/// Base class for all interactable objects.
/// Handles showing an interaction prompt and orienting it to face the player’s camera.
/// </summary>
public abstract class Interactable : MonoBehaviour
{
    [Header("Interaction Settings")]
    [Tooltip("Message shown to the player (not used for image-based prompts).")]
    public string promptMessage = "Press E to interact";

    [Tooltip("Prefab used as the visual interaction prompt (e.g., an 'E' icon).")]
    public GameObject interactionPromptPrefab;

    private GameObject currentPromptInstance;
    private Camera mainCamera;

    public abstract void Interact();

    protected virtual void Start()
    {
        mainCamera = Camera.main;
    }

    public virtual void ShowPrompt()
    {
        if (interactionPromptPrefab == null || currentPromptInstance != null)
            return;

        // Position prompt slightly above the object's center
        Vector3 spawnPosition = GetComponent<Collider>().bounds.center + Vector3.up * 3f;
        currentPromptInstance = Instantiate(interactionPromptPrefab, spawnPosition, Quaternion.identity);

        if (mainCamera != null)
        {
            Vector3 camEuler = mainCamera.transform.eulerAngles;
            currentPromptInstance.transform.rotation = Quaternion.Euler(camEuler.x, camEuler.y, 0f);
        }
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
}
