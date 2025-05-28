using UnityEngine;
using DG.Tweening;

public class PlatformController : MonoBehaviour
{
    public Transform platform;  // Assign your platform transform here
    public Vector3 upPosition;  // local position when platform is up
    public Vector3 downPosition; // local position when platform is down
    public float moveDuration = 1f;

    private bool isUp = true;

    private void Start()
    {
        // Initialize platform to upPosition
        platform.localPosition = upPosition;
    }

    public void MovePlatformDown()
    {
        if (isUp)
        {
            platform.DOLocalMove(downPosition, moveDuration);
            isUp = false;
        }
    }

    public void MovePlatformUp()
    {
        if (!isUp)
        {
            platform.DOLocalMove(upPosition, moveDuration);
            isUp = true;
        }
    }

    public bool IsPlatformUp()
    {
        return isUp;
    }
}
