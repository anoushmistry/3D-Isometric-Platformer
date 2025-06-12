using UnityEngine;

public class CameraAngleChangeComponent : MonoBehaviour
{
    [SerializeField] private float cameraYAngleChange;
    [SerializeField] private float duration;
    public float GetCameraAngleChangeValue()
    {
        return cameraYAngleChange;
    }
    public float GetDuration()
    {
        return duration;
    }
}
