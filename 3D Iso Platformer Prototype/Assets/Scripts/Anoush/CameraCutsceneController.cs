using UnityEngine;
using Cinemachine;
using System.Collections;

public class CameraCutsceneController : MonoBehaviour
{

    public static CameraCutsceneController instance;
    [SerializeField] private CinemachineVirtualCamera vcam_Player;
    [SerializeField] private CinemachineVirtualCamera vcam_Door;

    [SerializeField] private float doorFocusDuration = 5f;

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(instance);
            instance = this;
        }
    }
    public void PlayDoorCutscene()
    {
        StartCoroutine(FocusDoorAndReturn());
    }

    private IEnumerator FocusDoorAndReturn()
    {
        // Focus on the door
        vcam_Door.Priority = 20;
        vcam_Player.Priority = 10;

        yield return new WaitForSeconds(doorFocusDuration);

        // Return to player
        vcam_Door.Priority = 10;
        vcam_Player.Priority = 20;
    }
}
