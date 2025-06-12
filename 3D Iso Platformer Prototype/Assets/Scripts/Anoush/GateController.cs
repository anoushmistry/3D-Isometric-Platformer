using UnityEngine;
using System.Collections;

public class GateController : MonoBehaviour
{
    public Transform gateObject;
    public Vector3 loweredPositionOffset = new Vector3(0, -3f, 0);
    public float dropDuration = 1f;

    private Vector3 initialPosition;

    private void Start()
    {
        if (gateObject != null)
            initialPosition = gateObject.position;
    }

    public void DropGate()
    {
        StartCoroutine(DropGateCoroutine());
    }

    private IEnumerator DropGateCoroutine()
    {
        if (SoundManager.Instance != null)
        {
            SoundManager.Instance?.PlayGateDropSFX(transform.position);
        }

        Vector3 targetPosition = initialPosition + loweredPositionOffset;
        Vector3 startPosition = gateObject.position;
        float timer = 0f;

        while (timer < dropDuration)
        {
            gateObject.position = Vector3.Lerp(startPosition, targetPosition, timer / dropDuration);
            timer += Time.deltaTime;
            yield return null;
        }

        gateObject.position = targetPosition;
    }
}
