using UnityEngine;
using DG.Tweening;

public class Door : MonoBehaviour
{
    public void OpenDoor()
    {
        transform.DOMoveY(transform.position.y + 3f, 1f); // Moves up when activated
    }
}
