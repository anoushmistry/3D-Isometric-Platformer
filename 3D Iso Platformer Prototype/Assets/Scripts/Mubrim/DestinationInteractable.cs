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

    private GameObject customPromptInstance;

    private void Start()
    {
        playerInteraction = FindObjectOfType<PlayerInteraction>();
    }

    public override void Interact()
    {
        if (isOrbPlaced || playerInteraction == null || !playerInteraction.IsHoldingOrb())
            return;

        playerInteraction.PlaceOrb(transform.position + offset);
        isOrbPlaced = true;

        HidePrompt();
    }

    public override bool IsInteractable()
    {
        return !isOrbPlaced && playerInteraction != null && playerInteraction.IsHoldingOrb();
    }

    public override void ShowPrompt()
    {
        if (!IsInteractable() || interactionPromptPrefab == null || customPromptInstance != null)
            return;

        Vector3 spawnPosition = GetComponent<Collider>().bounds.center;

        if (Camera.main != null)
        {
            Vector3 directionToCamera = (Camera.main.transform.position - spawnPosition).normalized;
            spawnPosition += directionToCamera * 2f; // 2 units toward the camera
        }

        customPromptInstance = Instantiate(interactionPromptPrefab, spawnPosition, Quaternion.identity);
        customPromptInstance.name = "__OrbPrompt";
        interactionPromptPrefab.SetActive(true);

        if (Camera.main != null)
        {
            Vector3 camEuler = Camera.main.transform.eulerAngles;
            customPromptInstance.transform.rotation = Quaternion.Euler(camEuler.x, camEuler.y, 0f);
        }
    }

    public override void HidePrompt()
    {
        if (customPromptInstance != null)
        {
            Destroy(customPromptInstance);
            customPromptInstance = null;
        }
    }
}
