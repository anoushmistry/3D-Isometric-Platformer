using UnityEngine;

public class OrbPickupHandler : MonoBehaviour
{
    public void MoveToDestination(Vector3 destination, float speed)
    {
        StartCoroutine(LerpToPosition(destination, speed));
    }

    private System.Collections.IEnumerator LerpToPosition(Vector3 targetPos, float speed)
    {
        while (Vector3.Distance(transform.position, targetPos) > 0.05f)
        {
            transform.position = Vector3.Lerp(transform.position, targetPos, Time.deltaTime * speed);
            transform.Rotate(Vector3.up * 90f * Time.deltaTime, Space.Self); // Spin while moving
            yield return null;
        }

        transform.position = targetPos;
    }
}
