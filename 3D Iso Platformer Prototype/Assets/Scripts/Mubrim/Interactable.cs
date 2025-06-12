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

    /// <summary>
    /// Shows prompt at default offset (center + upward + a bit toward camera).
    /// </summary>
    public virtual void ShowPrompt()
    {
        if (interactionPromptPrefab == null || currentPromptInstance != null)
            return;

        Vector3 spawnPosition = GetComponent<Collider>().bounds.center + Vector3.up * 1.5f;

        if (mainCamera != null)
        {
            Vector3 directionToCamera = (mainCamera.transform.position - spawnPosition).normalized;
            spawnPosition += directionToCamera * 1f;
        }

        CreatePrompt(spawnPosition);
    }

    /// <summary>
    /// Shows prompt at a custom world position.
    /// </summary>
    public virtual void ShowPrompt(Vector3 customWorldPosition)
    {
        if (interactionPromptPrefab == null || currentPromptInstance != null)
            return;

        CreatePrompt(customWorldPosition);
    }

    private void CreatePrompt(Vector3 position)
    {
        currentPromptInstance = Instantiate(interactionPromptPrefab, position, Quaternion.identity);

        if (mainCamera != null)
        {
            Vector3 camEuler = mainCamera.transform.eulerAngles;
            currentPromptInstance.transform.rotation = Quaternion.Euler(camEuler.x, camEuler.y, 0f);
        }

        interactionPromptPrefab.SetActive(true);
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
