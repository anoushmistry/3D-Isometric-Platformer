using UnityEngine;

public class CameraAngleChangeComponent : MonoBehaviour
{
    [SerializeField] private float cameraYAngleChange;
    public float GetCameraAngleChangeValue()
    {
        return cameraYAngleChange;
    }
}
