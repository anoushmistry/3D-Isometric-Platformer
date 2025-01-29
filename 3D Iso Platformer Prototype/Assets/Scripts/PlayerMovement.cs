using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float speed = 5f;
    public CharacterController controller;
    private Vector3 moveDirection;

    // Rotation smooth time and velocity reference for SmoothDampAngle
    private float currentVelocity = 0.0f;
    public float rotationSmoothTime = 0.1f;  // Controls the smoothness of rotation

    void Update()
    {
        // Get input axes
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        // Get camera's forward and right vectors
        Vector3 camForward = Camera.main.transform.forward;
        Vector3 camRight = Camera.main.transform.right;

        // Ignore Y component (keep movement flat)
        camForward.y = 0;
        camRight.y = 0;

        // Normalize directions
        camForward.Normalize();
        camRight.Normalize();

        // Calculate movement direction relative to camera
        moveDirection = (camForward * vertical + camRight * horizontal).normalized;

        // Only rotate if there is movement
        if (moveDirection.magnitude >= 0.1f)
        {
            // Calculate the target angle based on the movement direction
            float targetAngle = Mathf.Atan2(moveDirection.x, moveDirection.z) * Mathf.Rad2Deg;

            // Smoothly interpolate between current and target rotation
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref currentVelocity, rotationSmoothTime);

            // Apply the smooth rotation
            transform.rotation = Quaternion.Euler(0, angle, 0);

            // Move the player
            controller.Move(moveDirection * speed * Time.deltaTime);
        }
    }
}
