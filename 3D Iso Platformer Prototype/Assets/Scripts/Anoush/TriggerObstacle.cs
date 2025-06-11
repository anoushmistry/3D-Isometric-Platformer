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
        if (other.CompareTag("Player"))
        {
            if (obstacle == null || collider == null || impulseSource == null)
            {
                Debug.LogError("Obstacle, Collider, or ImpulseSource is not assigned in the inspector.");
                return;
            }
            if (!isObstacleTriggered)
            {
                Vector3 directionToPlayer = (other.transform.position - obstacle.position).normalized;
                StartCoroutine(SmoothRotateObstacle(rotationToApply, duration, directionToPlayer)); // Smoothly rotate the obstacle over 1 second
                // Apply the rotation to the obstacle
                //obstacle.rotation = rotationToApply;
                //Debug.Log("Obstacle rotation applied: " + rotationToApply.eulerAngles);
                //collider.convex = true;
                ////impulseSource.GenerateImpulse();
                //// Generate impulse toward the player
                //impulseSource.GenerateImpulseAt(obstacle.position, directionToPlayer * 1f); // adjust magnitude if needed

                if (CameraCutsceneController.instance != null)
                {
                    CameraCutsceneController.instance.PlayObstacleCutscene();

                }
                isObstacleTriggered = true;


            }
        }
    }
    private IEnumerator SmoothRotateObstacle(Quaternion targetRotation, float duration, Vector3 direction)
    {
        yield return new WaitForSeconds(1f); // To wait negligible time cause camera blend
        Quaternion startRotation = obstacle.rotation;
        float time = 0f;

        while (time < duration)
        {
            time += Time.deltaTime;
            float t = time / duration;
            float easedT = t * t; // ease-in quadratic
            obstacle.rotation = Quaternion.Slerp(startRotation, targetRotation, easedT);
            yield return null;
        }


        obstacle.rotation = targetRotation; // Ensure it finishes exactly at the target
        Vector3 directionToPlayer = direction;
        collider.convex = true;
        impulseSource.GenerateImpulse(); // Generate impulse to shake the obstacle
        //impulseSource.GenerateImpulseAt(obstacle.position, directionToPlayer * 1f);
    }
}
