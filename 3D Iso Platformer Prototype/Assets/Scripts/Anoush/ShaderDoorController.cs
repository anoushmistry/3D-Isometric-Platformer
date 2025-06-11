using Cinemachine;
using DG.Tweening;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ShaderDoorController : Interactable
{
    public PlayerInteraction playerInteraction;
    private Animator m_animator;

    [SerializeField] private string sceneToLoad;
    private Material dissolveMaterial;
    private Coroutine spawnDelayCoroutine;
    private Coroutine loadSceneCoroutine;

    private CinemachineImpulseSource impulseSource;
    private bool isInteractable;
    public bool isDestinationDoor;
    public bool requiresOrbActivation;

    private void Start()
    {
        impulseSource = GetComponent<CinemachineImpulseSource>();
        m_animator = GetComponent<Animator>();
        dissolveMaterial = GetComponent<MeshRenderer>().material;

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
                    isInteractable = false;
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
                    isInteractable = false;
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
                isInteractable = false;
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

    private IEnumerator SpawnDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        DoorDissolveAppear();
        spawnDelayCoroutine = null;
        isInteractable = true;
    }

    void DoorDissolveAppear()
    {
        if (dissolveMaterial == null)
        {
            Debug.LogError("Dissolve material not set!");
            return;
        }

        dissolveMaterial.DOFloat(5f, "_NoiseStrength", 2f)
            .SetEase(Ease.InOutSine);

        float originalY = transform.position.y;

        transform.DOMoveY(originalY + 5f, 0.5f).SetEase(Ease.InOutSine);
        transform.DORotate(new Vector3(-70, transform.rotation.eulerAngles.y, transform.rotation.eulerAngles.z), 0.5f)
            .OnComplete(() =>
            {
                transform.DORotate(new Vector3(-90, transform.rotation.eulerAngles.y, transform.rotation.eulerAngles.z), 0.425f).SetEase(Ease.InOutSine);
                transform.DOMoveY(originalY, 0.425f).SetEase(Ease.InOutSine).OnComplete(() =>
                {
                    impulseSource?.GenerateImpulse();
                });
            });

        // 🔊 Start delayed sound from SoundManager
        StartCoroutine(PlayDoorSoundDelayed(1f));
    }

    void DoorDissolveDisappear()
    {
        if (dissolveMaterial == null)
        {
            Debug.LogError("Dissolve material not set!");
            return;
        }

        dissolveMaterial.DOFloat(0f, "_NoiseStrength", 2f)
            .SetEase(Ease.InOutSine);
    }

    public void SpeedUpAnim(float speed)
    {
        m_animator.speed = speed;
    }

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
            StopCoroutine(loadSceneCoroutine);
            loadSceneCoroutine = StartCoroutine(LoadNextScene(delay));
        }
    }

    private IEnumerator LoadNextScene(float delay)
    {
        yield return new WaitForSeconds(delay);
        SceneController.Instance.LoadScene(sceneToLoad);
        loadSceneCoroutine = null;
    }

    private IEnumerator PlayDoorSoundDelayed(float delay)
    {
        yield return new WaitForSeconds(delay);
        SoundManager.Instance?.PlayDoorBangSFX();
    }
}
