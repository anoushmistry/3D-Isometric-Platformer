using UnityEngine;

public class LightReceiver1 : MonoBehaviour
{
    public Door door; // Assign the Door in the Inspector

    public void ActivateReceiver()
    {
        door.OpenDoor();
    }
}
