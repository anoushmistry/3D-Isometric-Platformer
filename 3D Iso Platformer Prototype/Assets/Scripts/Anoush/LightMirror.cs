using UnityEngine;

public class LightMirror : Interactable
{
    public float rotationSpeed = 60f;
    public bool isObjective1Mirror;
    public bool isObjective2Mirror;

    public override void Interact()
    {
        PlayerInteraction player = FindObjectOfType<PlayerInteraction>();
        if (player != null)
        {
            player.EnterMirrorRotationMode(this);
            if(isObjective1Mirror)
            {
                CameraCutsceneController.instance.FocusObjective(ObjectiveList.Objective1);
            }
            else
            {
                CameraCutsceneController.instance.FocusObjective(ObjectiveList.Objective2);
            }
        }
    }

    public void RotateMirror(float direction)
    {
        transform.Rotate(Vector3.up, direction * rotationSpeed * Time.deltaTime);
        //Debug.Log($"Rotating mirror {direction * rotationSpeed * Time.deltaTime} degrees");
    }
    public void EndMirrorInteraction()
    {
        if (isObjective1Mirror)
        {
            CameraCutsceneController.instance.EndFocusObjective(ObjectiveList.Objective1);
        }
        else
        {
            CameraCutsceneController.instance.EndFocusObjective(ObjectiveList.Objective2);
        }
        
    }
}
