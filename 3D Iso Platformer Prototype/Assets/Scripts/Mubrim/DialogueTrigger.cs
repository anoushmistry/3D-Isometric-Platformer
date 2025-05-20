//Tells the system when to start a conversation.
using UnityEngine;

public class DialogueTrigger : Interactable
{
    public Dialogue dialogue; // Assign sentences in inspector

    public override void Interact()
    {
        DialogueManager.Instance.StartDialogue(dialogue.sentences);
    }
}
