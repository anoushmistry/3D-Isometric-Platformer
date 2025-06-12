using System.Collections;
using UnityEngine;

public class TriggerObstacle : MonoBehaviour
{
    [SerializeField] private Transform obstacle;
    [SerializeField] private MeshCollider collider;
    [SerializeField] private Quaternion rotationToApply;
    [SerializeField] private Cinemachine.CinemachineImpulseSource impulseSource;
    [SerializeField] private float duration;

    private bool isObstacleTriggered;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !isObstacleTriggered)
        {
            if (obstacle == null || collider == null || impulseSource == null)
            {
                Debug.LogError("Obstacle, Collider, or ImpulseSource is not assigned in the inspector.");
                return;
            }

            Vector3 directionToPlayer = (other.transform.position - obstacle.position).normalized;
            StartCoroutine(SmoothRotateObstacle(rotationToApply, duration, directionToPlayer));

            if (CameraCutsceneController.instance != null)
            {
                CameraCutsceneController.instance.PlayObstacleCutscene();
            }

            isObstacleTriggered = true;
        }
    }

    private IEnumerator SmoothRotateObstacle(Quaternion targetRotation, float duration, Vector3 direction)
    {
        yield return new WaitForSeconds(1f); // Wait for camera blend

        SoundManager.Instance?.PlayFallingTreeSFX();

        Quaternion startRotation = obstacle.rotation;
        float time = 0f;

        while (time < duration)
        {
            time += Time.deltaTime;
            float t = time / duration;
            float easedT = t * t; 
            obstacle.rotation = Quaternion.Slerp(startRotation, targetRotation, easedT);
            yield return null;
        }

        obstacle.rotation = targetRotation;
        collider.convex = true;
        impulseSource.GenerateImpulse(); // Camera shake
    }
}
