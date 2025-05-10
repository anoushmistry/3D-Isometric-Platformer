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
        //player = Camera.main.transform; // Replace with your player reference if needed
        initialRotation = switchHandle.localRotation;
        targetRotation = Quaternion.Euler(switchRotationAngle) * initialRotation;
    }

    void Update()
    {
        if (playerInRange && Input.GetKeyDown(interactKey))
        {
            ToggleSwitch();
        }

        // Smooth rotation
        Quaternion goalRotation = isOn ? targetRotation : initialRotation;
        switchHandle.localRotation = Quaternion.Slerp(switchHandle.localRotation, goalRotation, Time.deltaTime * rotationSpeed);
    }

    void ToggleSwitch()
    {
        isOn = !isOn;

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