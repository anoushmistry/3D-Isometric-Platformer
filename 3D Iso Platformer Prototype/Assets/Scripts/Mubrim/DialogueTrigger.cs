//Tells the system when to start a conversation.
using UnityEngine;

public class DialogueTrigger : Interactable
{
    public Dialogue dialogue; 
    private bool hasInteracted = false;

    public override void Interact()
    {
        if (hasInteracted || DialogueManager.Instance.IsDialogueActive())
            return;

        DialogueManager.Instance.StartDialogue(dialogue.sentences);
        hasInteracted = true;
    }
}
