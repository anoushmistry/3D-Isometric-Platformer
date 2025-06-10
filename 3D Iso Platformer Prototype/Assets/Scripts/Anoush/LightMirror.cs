using UnityEngine;

public class LightMirror : Interactable
{
    public float rotationSpeed = 60f;

    public override void Interact()
    {
        PlayerInteraction player = FindObjectOfType<PlayerInteraction>();
        if (player != null)
        {
            player.EnterMirrorRotationMode(this);
        }
    }

    public void RotateMirror(float direction)
    {
        transform.Rotate(Vector3.up, direction * rotationSpeed * Time.deltaTime);
        Debug.Log($"Rotating mirror {direction * rotationSpeed * Time.deltaTime} degrees");
    }
}
