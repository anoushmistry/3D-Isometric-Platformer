using UnityEngine;
using Cinemachine;
using System.Collections;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal; // For URP

public enum ObjectiveList
{
    Objective1,
    Objective2,
}
public class CameraCutsceneController : MonoBehaviour
{

    public static CameraCutsceneController instance;
    [SerializeField] private CinemachineVirtualCamera vcam_Player;
    [Header("For Level 1")] // Assuming this is for Level 1
    [SerializeField] private CinemachineVirtualCamera vcam_Door;
    [SerializeField] private CinemachineVirtualCamera vcam_Obstacle;
    [SerializeField] private float doorFocusDuration = 5f;
    [SerializeField] private float obstacleFocusDuration = 5f; // Optional, if you want different durations for obstacles

    [Header("For Level 2")]
    [SerializeField] private Volume postProcessingVolume; // Optional: If you want to change post-processing effects during cutscenes
    [SerializeField] private CinemachineVirtualCamera vCam_Objective1;
    [SerializeField] private CinemachineVirtualCamera vCam_Objective2;
    [SerializeField] private Camera mainCamera; // Main camera for the scene   
    [SerializeField] private Camera objectiveCamera; // Main camera for the scene   


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
        SoundManager.Instance?.StopFootstepLoop();
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
        SoundManager.Instance?.StopFootstepLoop();
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
    public void FocusObjective(ObjectiveList objectiveList) // For mirror in Level 2
    {
        if (postProcessingVolume != null && postProcessingVolume.profile != null)
        {
            DepthOfField dof;
            if (postProcessingVolume.profile.TryGet<DepthOfField>(out dof))
            {
                dof.active = false;
            }
        }
        if(objectiveList == ObjectiveList.Objective1)
        {
            // Focus on the door
            vCam_Objective1.Priority = 20;
            vcam_Player.Priority = 10;
        }
        else if (objectiveList == ObjectiveList.Objective2)
        {
            AnimateSplitViewCoroutine();
            //objectiveCamera.enabled = true; // Enable the objective camera
            //mainCamera.rect = new Rect(0f, 0f, 0.5f, 1f); // Left half
            //objectiveCamera.rect = new Rect(0.5f, 0f, 0.5f, 1f); // Right half
            // Focus on the door
            vCam_Objective1.Priority = 20;
            vcam_Player.Priority = 10;
        }
      

    }
    public void EndFocusObjective(ObjectiveList objectiveList)
    {
        if (objectiveList == ObjectiveList.Objective1)
        {
            // Focus on the door
            vCam_Objective1.Priority = 10;
            vcam_Player.Priority = 20;
        }
        else if (objectiveList == ObjectiveList.Objective2)
        {
            // Focus on the door
            vCam_Objective1.Priority = 10;
            vcam_Player.Priority = 20;
            AnimateCloseSplitViewCoroutine();
           // mainCamera.rect = new Rect(0f, 0f, 1f, 1f);
           // objectiveCamera.enabled = false; // Enable the objective camera
        }
        StartCoroutine(TurnOffDepthOfField(2f)); // Turn off Depth of Field after 2 seconds (Camera brain default blend time)

    }
    private IEnumerator TurnOffDepthOfField(float delay)
    {
        yield return new WaitForSeconds(delay);
        if (postProcessingVolume != null && postProcessingVolume.profile != null)
        {
            DepthOfField dof;
            if (postProcessingVolume.profile.TryGet<DepthOfField>(out dof))
            {
                dof.active = true;
            }
        }
    }
    public void AnimateSplitViewCoroutine()
    {
        StartCoroutine(SplitScreenAnimationCoroutine());
    }

    private IEnumerator SplitScreenAnimationCoroutine()
    {
        float duration = 1.2f;
        float timer = 0f;

        // Initial values
        Rect startMain = new Rect(0f, 0f, 1f, 1f);
        Rect endMain = new Rect(0f, 0f, 0.5f, 1f);

        Rect startObjective = new Rect(1f, 0f, 0.0f, 1f);
        Rect endObjective = new Rect(0.5f, 0f, 0.5f, 1f);

        objectiveCamera.enabled = true;

        while (timer < duration)
        {
            float t = timer / duration;

            mainCamera.rect = LerpRectSafe(startMain, endMain, t);
            //objectiveCamera.rect = LerpRect(startObjective, endObjective, t);
            Rect rect = LerpRectSafe(startObjective, endObjective, t);
            rect.width = Mathf.Max(0.01f, rect.width); // avoid zero width
            objectiveCamera.rect = rect;

            timer += Time.deltaTime;
            yield return null;
        }

        mainCamera.rect = endMain;
        objectiveCamera.rect = endObjective;
    }

    //private Rect LerpRect(Rect from, Rect to, float t)
    //{
    //    return new Rect(
    //        Mathf.Lerp(from.x, to.x, t),
    //        Mathf.Lerp(from.y, to.y, t),
    //        Mathf.Lerp(from.width, to.width, t),
    //        Mathf.Lerp(from.height, to.height, t)
    //    );
    //}
    private Rect LerpRectSafe(Rect from, Rect to, float t)
    {
        float width = Mathf.Lerp(from.width, to.width, t);
        float height = Mathf.Lerp(from.height, to.height, t);

        // Clamp minimum values
        width = Mathf.Max(width, 0.01f);
        height = Mathf.Max(height, 0.01f);

        return new Rect(
            Mathf.Lerp(from.x, to.x, t),
            Mathf.Lerp(from.y, to.y, t),
            width,
            height
        );
    }
    public void AnimateCloseSplitViewCoroutine()
    {
        StartCoroutine(CloseSplitScreenAnimationCoroutine());
    }

    private IEnumerator CloseSplitScreenAnimationCoroutine()
    {
        float duration = 1.2f;
        float timer = 0f;

        Rect startMain = new Rect(0f, 0f, 0.5f, 1f);
        Rect endMain = new Rect(0f, 0f, 1f, 1f);

        Rect startObjective = new Rect(0.5f, 0f, 0.5f, 1f);
        Rect endObjective = new Rect(1f, 0f, 0.0f, 1f); // Slide right offscreen

        while (timer < duration)
        {
            float t = timer / duration;

            mainCamera.rect = LerpRectSafe(startMain, endMain, t);
            objectiveCamera.rect = LerpRectSafe(startObjective, endObjective, t);

            timer += Time.deltaTime;
            yield return null;
        }

        mainCamera.rect = endMain;
        objectiveCamera.enabled = false; // Disable after transition
    }

}
