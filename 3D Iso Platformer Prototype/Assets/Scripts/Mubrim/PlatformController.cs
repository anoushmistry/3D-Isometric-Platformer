using UnityEngine;
using DG.Tweening;

public class PlatformController : MonoBehaviour
{
    public Transform platform;
    public Vector3 upPosition;
    public Vector3 downPosition;
    public float moveDuration = 1f;

    private bool isUp = true;

    private void Start()
    {
        platform.localPosition = upPosition;
    }

    public void MovePlatformDown()
    {
        if (isUp)
        {
            platform.DOLocalMove(downPosition, moveDuration);
            isUp = false;

            // 🔊 Play bridge move sound
            if (SoundManager.Instance != null)
                SoundManager.Instance.PlayBridgeMoveSFX(transform.position);
        }
    }

    public void MovePlatformUp()
    {
        if (!isUp)
        {
            platform.DOLocalMove(upPosition, moveDuration);
            isUp = true;

            // 🔊 Play bridge move sound
            if (SoundManager.Instance != null)
                SoundManager.Instance.PlayBridgeMoveSFX(transform.position);
        }
    }

    public bool IsPlatformUp()
    {
        return isUp;
    }
}
