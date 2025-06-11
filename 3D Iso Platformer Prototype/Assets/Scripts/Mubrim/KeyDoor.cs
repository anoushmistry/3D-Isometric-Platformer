using UnityEngine;
using DG.Tweening;

public class KeyDoor : Interactable
{
    //[SerializeField] private Transform leftDoor;
    //[SerializeField] private Transform rightDoor;
    [SerializeField] private GateController gate;
    [SerializeField] private float openDuration = 1.2f;

    [SerializeField]
    private string[] lockedDialogue = {
        "This door requires a key to open."
    };

    private bool isOpen = false;

    public override void Interact()
    {
        if (isOpen) return;
        if (PlayerInventory.Instance.HasKey)
        {
            PlayerInventory.Instance.UseKey();

            KeyUIController.Instance.HideKeyIcon();
            OpenDoor();
            gameObject.SetActive(false);
        }
        else
        {
            DialogueManager.Instance.StartDialogue(lockedDialogue);
        }
    }

    private void OpenDoor()
    {
        isOpen = true;

        gate.DropGate();
        // Rotate doors
        //leftDoor.DOLocalRotate(new Vector3(0f, -75f, 0f), openDuration, RotateMode.LocalAxisAdd);
        //rightDoor.DOLocalRotate(new Vector3(0f, 75f, 0f), openDuration, RotateMode.LocalAxisAdd);

        // Disable prompt after opening
        if (interactionPromptPrefab != null)
            interactionPromptPrefab.SetActive(false);
    }
}
