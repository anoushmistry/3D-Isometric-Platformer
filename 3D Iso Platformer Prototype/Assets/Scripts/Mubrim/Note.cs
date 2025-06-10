using UnityEngine;

public class Note : Interactable
{
    [TextArea(3, 10)] public string noteText;

    public override void Interact()
    {
        NoteUI.Instance.ShowNote(noteText);
    }
}
