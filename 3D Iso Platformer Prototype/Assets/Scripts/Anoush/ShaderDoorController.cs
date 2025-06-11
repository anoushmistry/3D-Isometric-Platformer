using Cinemachine;
using DG.Tweening;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class ShaderDoorController : Interactable
{
    public PlayerInteraction playerInteraction;
    private Animator m_animator;
    private int m_animIDOpen;

    [SerializeField] private string sceneToLoad;
    private Material dissolveMaterial;
    private Coroutine spawnDelayCoroutine;
    private Coroutine loadSceneCoroutine;
    [Header("Audio")]
    [SerializeField] private AudioSource doorAudioSource;
    [SerializeField] private AudioClip doorBangClip;

    private CinemachineImpulseSource impulseSource;
    private bool isInteractable;
    public bool isDestinationDoor;
    public bool requiresOrbActivation;
    private void Start()
    {

        impulseSource = GetComponent<CinemachineImpulseSource>();
        m_animator = GetComponent<Animator>();
        dissolveMaterial = GetComponent<MeshRenderer>().material; // Creates a unique copy
        // m_animIDOpen = Animator.StringToHash("Door Spawn In");
        if (!isDestinationDoor)
        {
            if (spawnDelayCoroutine == null)
                spawnDelayCoroutine = StartCoroutine(SpawnDelay(0.5f));
        }
        else
        {
            isInteractable = true;
        }

    }
    public override void Interact()
    {
        if (isDestinationDoor)
        {
            if (requiresOrbActivation)
            {
                if (isInteractable && playerInteraction != null && playerInteraction.HasPlacedOrb())
                {
                    DoorDissolveDisappear();
                    LoadScene(2f);
                    isInteractable = false; // Prevent further interactions until the door disappears
                }
                else
                {
                    Debug.LogError("Door is not interactable yet! Put an orb in the pedestal.");
                }
            }
            else
            {
                if (isInteractable && playerInteraction != null)
                {
                    DoorDissolveDisappear();
                    LoadScene(2f);
                    isInteractable = false; // Prevent further interactions until the door disappears
                }
                else
                {
                    Debug.LogError("Is not interactable or player interaction component is null");
                }

            }
            
        }
        else if (!isDestinationDoor)
        {
            if (isInteractable)
            {
                DoorDissolveDisappear();
                LoadScene(2f);
                isInteractable = false; // Prevent further interactions until the door disappears
            }
            else
            {
                Debug.LogError("Door is not interactable yet! Wait for the spawn delay.");
            }
        }

    }
    public override void ShowPrompt()
    {
        base.ShowPrompt();
    }

    //private void Update()
    //{
    //    if (Input.GetButtonDown("Submit"))
    //    {
    //        if (playerInteraction != null && playerInteraction.HasPlacedOrb())
    //        {
    //            m_animator.SetTrigger(m_animIDOpen);
    //            Debug.Log("Door Opened!");
    //            LoadNextScene();

    //        }
    //        else
    //        {
    //            Debug.Log("Door Locked! Place the Orb first.");
    //        }
    //    }
    //}
    private IEnumerator SpawnDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        DoorDissolveAppear();
        spawnDelayCoroutine = null; // Reset coroutine reference
        isInteractable = true;
        yield break;
    }
    void DoorDissolveAppear()
    {
        if (dissolveMaterial == null)
        {
            Debug.LogError("Dissolve material not set!");
            return;
        }

        // 🔄 Begin the dissolve animation
        dissolveMaterial.DOFloat(5f, "_NoiseStrength", 2f)
            .SetEase(Ease.InOutSine)
            .OnComplete(() =>
            {
                Debug.Log("Dissolve animation complete!");
            });

        float originalY = transform.position.y;

        // Animate upward and rotate
        transform.DOMoveY(originalY + 5f, 0.5f).SetEase(Ease.InOutSine);
        transform.DORotate(new Vector3(-70, transform.rotation.eulerAngles.y, transform.rotation.eulerAngles.z), 0.5f)
            .OnComplete(() =>
            {
                transform.DORotate(new Vector3(-90, transform.rotation.eulerAngles.y, transform.rotation.eulerAngles.z), 0.425f).SetEase(Ease.InOutSine);
                transform.DOMoveY(originalY, 0.425f).SetEase(Ease.InOutSine).OnComplete(() =>
                {
                    impulseSource?.GenerateImpulse(); // Camera shake
                });
            });

        // 🔊 Start delayed door sound
        if (doorAudioSource != null && doorBangClip != null)
        {
            StartCoroutine(PlayDoorSoundDelayed(1f));
        }
        else
        {
            Debug.LogWarning("Missing door bang audio or source.");
        }
    }

    void DoorDissolveDisappear()
    {
        if (dissolveMaterial == null)
        {
            Debug.LogError("Dissolve material not set!");
            return;
        }

        dissolveMaterial.DOFloat(0f, "_NoiseStrength", 2f)
            .SetEase(Ease.InOutSine)
            .OnComplete(() =>
            {
                Debug.Log("Dissolve animation complete!");
            });
    }

    public void SpeedUpAnim(float speed)
    {
        m_animator.speed = speed;
    }
    //public void PlayCameraImpulse()
    //{
    //    if (impulseSource != null)
    //    {
    //        impulseSource.GenerateImpulse(); // ✅ This correctly sends the impulse
    //        Debug.Log("Camera Impulse Generated");
    //    }
    //}
    void LoadScene(float delay)
    {
        if (string.IsNullOrEmpty(sceneToLoad))
        {
            Debug.LogError("Scene to load is not set!");
            return;
        }
        if (loadSceneCoroutine == null)
        {
            loadSceneCoroutine = StartCoroutine(LoadNextScene(delay));
        }
        else
        {
            StopCoroutine(loadSceneCoroutine); // Stop any existing coroutine
            loadSceneCoroutine = null;
            loadSceneCoroutine = StartCoroutine(LoadNextScene(delay));
        }
    }

    private IEnumerator LoadNextScene(float delay)
    {
        yield return new WaitForSeconds(delay);
        SceneController.Instance.LoadScene(sceneToLoad);
        loadSceneCoroutine = null; // Reset coroutine reference
        yield break;
    }

    private IEnumerator PlayDoorSoundDelayed(float delay)
    {
        yield return new WaitForSeconds(delay);
        doorAudioSource.PlayOneShot(doorBangClip);
    }

}
