using UnityEngine;
using Cinemachine;
using System.Collections;

public class CameraCutsceneController : MonoBehaviour
{

    public static CameraCutsceneController instance;
    [SerializeField] private CinemachineVirtualCamera vcam_Player;
    [SerializeField] private CinemachineVirtualCamera vcam_Door;
    [SerializeField] private CinemachineVirtualCamera vcam_Obstacle;
    [SerializeField] private float doorFocusDuration = 5f;
    [SerializeField] private float obstacleFocusDuration = 5f; // Optional, if you want different durations for obstacles

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
            return;
        }

        instance = this;
        // Optional: If this needs to persist across scenes
        // DontDestroyOnLoad(this.gameObject);
    }
    public void PlayDoorCutscene()
    {
        StartCoroutine(FocusDoorAndReturn());
    }
    public void PlayObstacleCutscene()
    {
        StartCoroutine(FocusObstacleAndReturn());
    }

    private IEnumerator FocusDoorAndReturn()
    {
        PlayerMovement.Instance.LockInput = true;
        // Focus on the door
        vcam_Door.Priority = 20;
        vcam_Player.Priority = 10;

        yield return new WaitForSeconds(doorFocusDuration);

        // Return to player
        vcam_Door.Priority = 10;
        vcam_Player.Priority = 20;

        yield return new WaitForSeconds(2f); // This is the constant blend time for the camera switching in the Cinemachine Brain Settings
        PlayerMovement.Instance.LockInput = false; // Unlock player input after the cutscene
    }
    private IEnumerator FocusObstacleAndReturn()
    {
        PlayerMovement.Instance.LockInput = true;
        // Focus on the door
        vcam_Obstacle.Priority = 20;
        vcam_Player.Priority = 10;

        yield return new WaitForSeconds(obstacleFocusDuration);

        // Return to player
        vcam_Obstacle.Priority = 10;
        vcam_Player.Priority = 20;

        yield return new WaitForSeconds(2f); // This is the constant blend time for the camera switching in the Cinemachine Brain Settings
        PlayerMovement.Instance.LockInput = false; // Unlock player input after the cutscene
    }
}
