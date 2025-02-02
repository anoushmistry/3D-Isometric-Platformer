using UnityEngine;

public class LightReceiver : MonoBehaviour
{
    public Door door; // Assign the Door in the Inspector

    public void ActivateReceiver()
    {
        door.OpenDoor();
    }
}
