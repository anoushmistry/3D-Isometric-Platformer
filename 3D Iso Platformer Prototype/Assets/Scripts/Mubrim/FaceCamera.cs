using UnityEngine;

public class FaceCamera : MonoBehaviour
{
    private Camera mainCam;

    void Start()
    {
        mainCam = Camera.main;
    }

    void LateUpdate()
    {
        if (mainCam == null) return;

        Vector3 direction = mainCam.transform.forward;
        transform.rotation = Quaternion.LookRotation(direction);
    }
}
