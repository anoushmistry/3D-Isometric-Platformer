using System.Collections;
using UnityEngine;

public class TriggerObstacle : MonoBehaviour
{
    [SerializeField] private Transform obstacle;
    [SerializeField] private MeshCollider collider;
    [SerializeField] private Quaternion rotationToApply;
    [SerializeField] private Cinemachine.CinemachineImpulseSource impulseSource;
    [SerializeField] private float duration = 6f;

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
        yield return new WaitForSeconds(1f); // Optional delay before animation starts

        // Delay the sound separately
        StartCoroutine(DelayedFallingTreeSound(1.0f));

        Quaternion startRotation = obstacle.rotation;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(elapsed / duration);
            float easedT = Mathf.SmoothStep(0, 1, t);
            obstacle.rotation = Quaternion.Lerp(startRotation, targetRotation, easedT);
            yield return null;
        }

        obstacle.rotation = targetRotation;
        collider.convex = true;
        impulseSource.GenerateImpulse(); // Camera shake
    }

    private IEnumerator DelayedFallingTreeSound(float delay)
    {
        yield return new WaitForSeconds(delay);
        SoundManager.Instance?.PlayFallingTreeSFX();
    }
}
