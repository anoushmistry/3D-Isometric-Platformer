using UnityEngine;

public class FloatingKey : MonoBehaviour
{
    [Header("Rotation Settings")]
    public float rotationSpeed = 30f; // Degrees per second

    [Header("Floating Settings")]
    public float floatAmplitude = 0.2f;  // How much it moves up and down
    public float floatFrequency = 1f;    // How fast it moves up and down

    [Header("Activation")]
    public KeyCode activateKey = KeyCode.R;

    private bool isAnimating = false;
    private Vector3 startPos;

    private void Start()
    {
        startPos = transform.position;
    }

    private void Update()
    {
        if (Input.GetKeyDown(activateKey))
        {
            isAnimating = !isAnimating;
        }

        if (isAnimating)
        {
            Animate();
        }
    }

    private void Animate()
    {
        // Rotate around Y axis
        transform.Rotate(Vector3.up, rotationSpeed * Time.deltaTime, Space.World);

        // Float up and down using sine wave
        float newY = startPos.y + Mathf.Sin(Time.time * floatFrequency) * floatAmplitude;
        Vector3 pos = transform.position;
        pos.y = newY;
        transform.position = pos;
    }
}
