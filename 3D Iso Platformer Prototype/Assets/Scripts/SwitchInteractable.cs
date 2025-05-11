using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class SwitchInteractable : Interactable
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
    private Coroutine rotateCoroutine;

    void Start()
    {
        initialRotation = switchHandle.localRotation;
        targetRotation = Quaternion.Euler(switchRotationAngle) * initialRotation;
    }
    public override void Interact()
    {
        isOn = !isOn;

        // Trigger events based on switch state
        if (isOn)
        {
            OnSwitchActivated?.Invoke();
            rotateCoroutine = StartCoroutine(RotateSwitch());
        }
        //else
        //    OnSwitchDeactivated?.Invoke();

        

    }

    private IEnumerator RotateSwitch()
    {
        Quaternion startRotation = switchHandle.localRotation;
        Quaternion goalRotation = isOn ? targetRotation : initialRotation;
        float t = 0f;

        while (t < 1f) 
        {
            t += Time.deltaTime * rotationSpeed;
            switchHandle.localRotation = Quaternion.Slerp(startRotation, goalRotation, t);
            yield return null; // Wait for the next frame
        }

        switchHandle.localRotation = goalRotation; // Ensure final alignment
    }

    //void Update()
    //{
    //    // Check if player is in range and the interact key is pressed
    //    if (playerInRange && Input.GetKeyDown(interactKey))
    //    {
    //        ToggleSwitch();
    //    }

    //    // Smooth rotation of the switch handle
    //    Quaternion goalRotation = isOn ? targetRotation : initialRotation;
    //    switchHandle.localRotation = Quaternion.Slerp(switchHandle.localRotation, goalRotation, Time.deltaTime * rotationSpeed);
    //}

    //void ToggleSwitch()
    //{
    //    isOn = !isOn;

    //    // Trigger events based on switch state
    //    if (isOn)
    //        OnSwitchActivated?.Invoke();
    //    else
    //        OnSwitchDeactivated?.Invoke();
    //}

    //void OnTriggerStay(Collider other)
    //{
    //    if (other.CompareTag("Player"))
    //    {
    //        playerInRange = true;
    //    }
    //}

    //void OnTriggerExit(Collider other)
    //{
    //    if (other.CompareTag("Player"))
    //    {
    //        playerInRange = false;
    //    }
    //}
}
