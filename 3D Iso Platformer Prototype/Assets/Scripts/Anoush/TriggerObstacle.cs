using System.Collections;
using UnityEngine;

public class TriggerObstacle : MonoBehaviour
{
    [SerializeField] private Transform obstacle;
    [SerializeField] private MeshCollider collider;
    [SerializeField] private Quaternion rotationToApply;
    [SerializeField] private Cinemachine.CinemachineImpulseSource impulseSource;
    [SerializeField] private float duration = 3f; // Set this higher for a slower fall

    private bool isObstacleTriggered;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !isObstacleTriggered)
        {
            if (obstacle == null || collider == null || impulseSource == null)
            {
                Debug.LogError("Obstacle, Collider, or ImpulseSource is not assigned.");
                return;
            }

            StartCoroutine(SmoothRotateObstacle(rotationToApply, duration));

            if (CameraCutsceneController.instance != null)
                CameraCutsceneController.instance.PlayObstacleCutscene();

            isObstacleTriggered = true;
        }
    }

    private IEnumerator SmoothRotateObstacle(Quaternion targetRotation, float duration)
    {
        yield return new WaitForSeconds(1f); // Delay to allow camera blend

        SoundManager.Instance?.PlayFallingTreeSFX();

        Quaternion startRotation = obstacle.rotation;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(elapsed / duration);
            float easedT = Mathf.SmoothStep(0, 1, t); // Smooth easing
            obstacle.rotation = Quaternion.Lerp(startRotation, targetRotation, easedT);
            yield return null;
        }

        obstacle.rotation = targetRotation;
        collider.convex = true;
        impulseSource.GenerateImpulse(); // Shake camera
    }
}
