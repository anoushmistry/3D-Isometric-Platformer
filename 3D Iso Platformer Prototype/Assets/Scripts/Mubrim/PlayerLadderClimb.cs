using UnityEngine;

public class PlayerLadderClimb : MonoBehaviour
{
    private CharacterController controller;
    private PlayerMovement playerMovement;
    private bool isClimbing = false;
    private float climbSpeed = 3f;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        playerMovement = GetComponent<PlayerMovement>();
    }

    void Update()
    {
        if (!isClimbing) return;

        float verticalInput = Input.GetAxis("Vertical");
        Vector3 climbDirection = new Vector3(0, verticalInput * climbSpeed, 0);

        controller.Move(climbDirection * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Ladder"))
        {
            isClimbing = true;
            playerMovement.LockInput = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Ladder"))
        {
            isClimbing = false;
            playerMovement.LockInput = false;
        }
    }
}
