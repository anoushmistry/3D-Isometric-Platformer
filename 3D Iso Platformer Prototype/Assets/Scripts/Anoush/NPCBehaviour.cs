using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Audio;

public class NPCBehaviour : MonoBehaviour
{
    [Header("Dialogue Control")]
    public DialogueTrigger dialogueTrigger;
    public Animator npcAnimator;

    [Header("Player Detection")]
    public float lookRadius = 5f;
    public float rotationSpeed = 5f;

    private Transform player;
    private bool isTalking = false;

    [Header("Random Kick Idle")]
    public float minIdleDelay = 8f;
    public float maxIdleDelay = 15f;
    public string kickIdleTrigger = "SadIdleTrigger";

    private Coroutine idleKickCoroutine;

    [Header("Talk Animation Settings")]
    public string[] talkAnimationTriggers; // e.g. "Talk1", "Talk2", "Talk3"
    private int dialogueCount = 0;

    [SerializeField] private List<AudioClip> dialogueAudioClips = new List<AudioClip>();
    private AudioSource audioSource;

    [Header("NPC Highlighter")]
    [SerializeField] private GameObject highlighter;

    private void Start()
    {
        player = PlayerMovement.Instance.transform;
        audioSource = GetComponent<AudioSource>();
        idleKickCoroutine = StartCoroutine(PlayRandomKickIdle());
        if (dialogueTrigger != null)
        {
            // Subscribe to dialogue events
            DialogueManager.Instance.OnDialogueStart += OnDialogueStart;
            DialogueManager.Instance.OnDialogueEnd += OnDialogueEnd;
            DialogueManager.Instance.OnSentenceChanged += OnSentenceChanged;
        }

    }
    private void Update()
    {
        if (player == null) return;

        float distance = Vector3.Distance(player.position, transform.position);

        // Look at player if nearby and not already talking
        if (distance <= lookRadius && !isTalking)
        {
            LookAtPlayer();
        }
    }

    private void LookAtPlayer()
    {
        Vector3 direction = (player.position - transform.position).normalized;
        direction.y = 0f; // Keep it horizontal
        Quaternion lookRotation = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * rotationSpeed);
    }

    private void OnDialogueStart()
    {
        isTalking = true;
        //npcAnimator.SetTrigger("Talk");
        npcAnimator.SetBool("Idle", false);
        dialogueCount++;
        audioSource.resource = dialogueAudioClips[0];
        audioSource.Play();
        // Every 2 dialogues, use a random animation
        if (dialogueCount % 2 == 0 && talkAnimationTriggers.Length > 0)
        {
            int randomIndex = Random.Range(0, talkAnimationTriggers.Length);
            npcAnimator.SetTrigger(talkAnimationTriggers[randomIndex]);
        }
        else
        {
            // Use default talk animation otherwise
            npcAnimator.SetTrigger("Talk 1");
        }
    }

    private void OnDialogueEnd()
    {
        isTalking = false;
        npcAnimator.SetBool("Idle", true);
        audioSource.Stop();
        highlighter.gameObject.SetActive(false);
        //npcAnimator.SetTrigger("Idle");
    }

    private void OnDestroy()
    {
        // Clean up event subscription
        if (DialogueManager.Instance != null)
        {
            DialogueManager.Instance.OnDialogueStart -= OnDialogueStart;
            DialogueManager.Instance.OnDialogueEnd -= OnDialogueEnd;
            DialogueManager.Instance.OnSentenceChanged += OnSentenceChanged;
        }
        if (idleKickCoroutine != null)
            StopCoroutine(idleKickCoroutine);
    }
    private IEnumerator PlayRandomKickIdle()
    {
        while (true)
        {
            // Wait until not talking
            while (isTalking)
                yield return null;

            float waitTime = Random.Range(minIdleDelay, maxIdleDelay);
            yield return new WaitForSeconds(waitTime);

            // Only play if STILL not talking
            if (!isTalking)
            {
                npcAnimator.SetTrigger("SadIdleTrigger");
            }
        }
    }
    private void OnSentenceChanged(int index)
    {
        audioSource.resource = dialogueAudioClips[index];
        audioSource.Play();
        if (index % 2 == 0 && isTalking)
        {
            if (talkAnimationTriggers.Length > 0)
            {
                string randomTrigger = talkAnimationTriggers[Random.Range(0, talkAnimationTriggers.Length)];
                npcAnimator.SetTrigger(randomTrigger);
            }
        }
        if(index == 4)
        {
            CameraCutsceneController.instance.PlayDoorCutscene();
        }
    }
}
