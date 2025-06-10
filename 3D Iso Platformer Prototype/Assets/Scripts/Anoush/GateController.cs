using System.Collections;
using UnityEngine;

public class GateController : MonoBehaviour
{
    public Vector3 openOffset = new Vector3(0, 3f, 0);
    public float moveSpeed = 2f;

    private Vector3 closedPosition;
    private Vector3 targetPosition;

    [SerializeField] private SwitchInteractable attachedSwitchInteractable;
    private Coroutine moveCoroutine;

    void Start()
    {
        closedPosition = transform.position;
        targetPosition = closedPosition;
    }

    public void OpenDoor()
    {
        targetPosition = closedPosition + openOffset;
        StartDoorMovement(() =>
        {
            attachedSwitchInteractable?.SetInteractable(true);
        });
    }

    public void CloseDoor()
    {
        targetPosition = closedPosition;
        StartDoorMovement(() =>
        {
            attachedSwitchInteractable?.SetInteractable(true);
        });
    }

    private void StartDoorMovement(System.Action onComplete)
    {
        if (moveCoroutine != null)
            StopCoroutine(moveCoroutine);

        moveCoroutine = StartCoroutine(MoveDoor(targetPosition, onComplete));
    }

    private IEnumerator MoveDoor(Vector3 target, System.Action onComplete)
    {
        while (Vector3.Distance(transform.position, target) > 0.01f)
        {
            transform.position = Vector3.MoveTowards(transform.position, target, moveSpeed * Time.deltaTime);
            yield return null;
        }

        transform.position = target;

        onComplete?.Invoke();
    }
}
