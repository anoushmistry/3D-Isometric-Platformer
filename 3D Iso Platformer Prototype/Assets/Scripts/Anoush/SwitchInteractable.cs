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
    // [SerializeField] private Transform player;
    private Quaternion initialRotation;
    private Quaternion targetRotation;
    private Coroutine rotateCoroutine;

    [Header("Tutorial Level")]
    [Tooltip("Set this to true if this switch is part of the tutorial level. It will trigger a text fade out when activated.")]
    [SerializeField] private bool IsTutorialLevel;
    [Tooltip("Don't Use or assign this unless it's the tutorial level switch")]
    [SerializeField] private TextFade textFadeComponent;
    private bool isInInteractable { get; set; }

    void Start()
    {
        initialRotation = switchHandle.localRotation;
        targetRotation = Quaternion.Euler(switchRotationAngle) * initialRotation;
        isInInteractable = true;
    }
    public override void Interact()
    {
        if (isInInteractable)
        {
            isOn = !isOn;

            StartCoroutine(RotateSwitch());

            if (IsTutorialLevel)
                if (textFadeComponent != null)
                    textFadeComponent.FadeOut();
                else
                    Debug.LogWarning("TextFade component is not assigned in the inspector.");
            //if (isOn)
            //{
            //    rotateCoroutine = StartCoroutine(RotateSwitch());
            //    OnSwitchActivated?.Invoke();
            //}
            //else
            //{
            //    rotateCoroutine = StartCoroutine(RotateSwitch());
            //    OnSwitchDeactivated?.Invoke();
            //}
        }

    }

    private IEnumerator RotateSwitch()
    {
        isInInteractable = false; // Prevent further interactions during rotation
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
        if (isOn)
        {
            OnSwitchActivated?.Invoke();
        }
        else
        {
            OnSwitchDeactivated?.Invoke();
        }
    }
    public void SetInteractable(bool value)
    {
        isInInteractable = value;
    }
}
