using UnityEngine;

public class DialogueTrigger : Interactable
{
    public Dialogue dialogue;
    private bool hasInteracted = false;

    public override void Interact()
    {
        if (hasInteracted || DialogueManager.Instance.IsDialogueActive())
            return;

       DialogueManager.Instance.StartDialogue(dialogue.sentences, OnDialogueFinished);
       hasInteracted = true;
    }


    private void OnDialogueFinished()
    {
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
