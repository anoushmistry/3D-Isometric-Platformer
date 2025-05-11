using UnityEngine;
using UnityEngine.Events;

public class SwitchInteractable : MonoBehaviour
{
    [Header("Switch Rotation")]
    public Transform switchHandle;
    public Vector3 switchRotationAngle = new Vector3(-45f, 0, 0);
    public float rotationSpeed = 5f;

    [Header("Interaction Settings")]
    public KeyCode interactKey = KeyCode.E;

    [Header("Events")]
    public UnityEvent OnSwitchActivated;
    public UnityEvent OnSwitchDeactivated;

    private bool isOn = false;
    private bool playerInRange = false;
    [SerializeField] private Transform player;
    private Quaternion initialRotation;
    private Quaternion targetRotation;

    void Start()
    {
        initialRotation = switchHandle.localRotation;
        targetRotation = Quaternion.Euler(switchRotationAngle) * initialRotation;
    }

    void Update()
    {
        // Check if player is in range and the interact key is pressed
        if (playerInRange && Input.GetKeyDown(interactKey))
        {
            ToggleSwitch();
        }

        // Smooth rotation of the switch handle
        Quaternion goalRotation = isOn ? targetRotation : initialRotation;
        switchHandle.localRotation = Quaternion.Slerp(switchHandle.localRotation, goalRotation, Time.deltaTime * rotationSpeed);
    }

    void ToggleSwitch()
    {
        isOn = !isOn;

        // Trigger events based on switch state
        if (isOn)
            OnSwitchActivated?.Invoke();
        else
            OnSwitchDeactivated?.Invoke();
    }

    void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
        }
    }
}
