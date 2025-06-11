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

    [Header("Audio")]
    public AudioSource switchAudioSource;
    public AudioClip switchSound;

    [Header("Tutorial Level")]
    [Tooltip("Set this to true if this switch is part of the tutorial level. It will trigger a text fade out when activated.")]
    [SerializeField] private bool IsTutorialLevel;
    [Tooltip("Don't Use or assign this unless it's the tutorial level switch")]
    [SerializeField] private TextFade textFadeComponent;

    private bool isOn = false;
    private bool isInInteractable { get; set; }
    private Quaternion initialRotation;
    private Quaternion targetRotation;

    void Start()
    {
        initialRotation = switchHandle.localRotation;
        targetRotation = Quaternion.Euler(switchRotationAngle) * initialRotation;
        isInInteractable = true;
    }

    public override void Interact()
    {
        if (!isInInteractable) return;

        isOn = !isOn;
        StartCoroutine(RotateSwitchWithSound());

        if (IsTutorialLevel && textFadeComponent != null)
            textFadeComponent.FadeOut();
    }

    private IEnumerator RotateSwitchWithSound()
    {
        isInInteractable = false;

        if (switchAudioSource != null && switchSound != null)
        {
            switchAudioSource.PlayOneShot(switchSound);
            yield return new WaitForSeconds(switchSound.length);
        }

        Quaternion startRotation = switchHandle.localRotation;
        Quaternion goalRotation = isOn ? targetRotation : initialRotation;
        float t = 0f;

        while (t < 1f)
        {
            t += Time.deltaTime * rotationSpeed;
            switchHandle.localRotation = Quaternion.Slerp(startRotation, goalRotation, t);
            yield return null;
        }

        switchHandle.localRotation = goalRotation;

        if (isOn) OnSwitchActivated?.Invoke();
        else OnSwitchDeactivated?.Invoke();

        isInInteractable = true;
    }

    public void SetInteractable(bool value)
    {
        isInInteractable = value;
    }
}
