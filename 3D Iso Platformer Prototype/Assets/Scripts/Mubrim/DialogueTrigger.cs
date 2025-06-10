using UnityEngine;

public class DialogueTrigger : Interactable
{
    public Dialogue dialogue;
    private bool hasInteracted = false;

    public override void Interact()
    {
        if (hasInteracted || SceneController.Instance.DialogueManager.IsDialogueActive())
            return;

       SceneController.Instance.DialogueManager.StartDialogue(dialogue.sentences, OnDialogueFinished);
    }

    private void OnDialogueFinished()
    {
        hasInteracted = true;
        HidePrompt();
    }

    public override void ShowPrompt()
    {
        if (hasInteracted) return;
        base.ShowPrompt();
    }

    public override bool IsInteractable()
    {
        return !hasInteracted;
    }
}
