using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class SwitchInteractable : Interactable
{
    [Header("Switch Push Down")]
    public Transform switchHandle;
    //public Vector3 switchRotationAngle = new Vector3(-45f, 0, 0);
    //public float rotationSpeed = 5f;
    [SerializeField] private float pushDownSpeed;
    [SerializeField] private Vector3 switchOffset;

    [Header("Interaction Settings")]
    public KeyCode interactKey = KeyCode.E;

    [Header("Events")]
    public UnityEvent OnSwitchActivated;
    //public UnityEvent OnSwitchDeactivated;

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

    
    private bool isInteractable { get; set; }

    void Start()
    {
        //initialRotation = switchHandle.localRotation;
        //targetRotation = Quaternion.Euler(switchRotationAngle) * initialRotation;
        isInteractable = true;
    }
    public override void Interact()
    {
        if (isInteractable)
        {
            isOn = true;

            //StartCoroutine(RotateSwitch());
            StartCoroutine(MoveSwitchDown());

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

    //private IEnumerator RotateSwitch()
    //{
    //    isInInteractable = false; // Prevent further interactions during rotation
    //    Quaternion startRotation = switchHandle.localRotation;
    //    Quaternion goalRotation = isOn ? targetRotation : initialRotation;
    //    float t = 0f;

    //    while (t < 1f)
    //    {
    //        t += Time.deltaTime * rotationSpeed;
    //        switchHandle.localRotation = Quaternion.Slerp(startRotation, goalRotation, t);
    //        yield return null; // Wait for the next frame
    //    }

    //    switchHandle.localRotation = goalRotation; // Ensure final alignment
    //    if (isOn)
    //    {
    //        OnSwitchActivated?.Invoke();
    //    }
    //    else
    //    {
    //        OnSwitchDeactivated?.Invoke();
    //    }
    //}
    private IEnumerator MoveSwitchDown()
    {

        SetInteractable(false); // Prevent interaction during motion
        Vector3 startPosition = switchHandle.localPosition;
        Vector3 endPosition =  startPosition + switchOffset; // Adjust offset based on model
        float t = 0f;

        while (t < 1f)
        {
            t += Time.deltaTime * pushDownSpeed;
            switchHandle.localPosition = Vector3.Lerp(startPosition, endPosition, t);
            yield return null;
        }

        switchHandle.localPosition = endPosition;

        if (isOn)
        {
            OnSwitchActivated?.Invoke();
        }
        //else
        //{
        //    OnSwitchDeactivated?.Invoke();
        //}
        
        //isInInteractable = false; // Re-enable interaction
        HidePrompt();
    }
    public void SetInteractable(bool value)
    {
        isInteractable = value;
    }
    public override void ShowPrompt()
    {
        if (!isInteractable) return;
        base.ShowPrompt();
    }
    public override void HidePrompt()
    {
        base.HidePrompt();
    }
}
