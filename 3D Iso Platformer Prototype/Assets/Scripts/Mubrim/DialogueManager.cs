//Handles the display, typing effect, advancing through lines, and UI updates.
using System;
using System.Collections;
using UnityEngine;
using TMPro;
using DG.Tweening;
using System.Collections.Generic;

public class DialogueManager : MonoBehaviour
{
    public static DialogueManager Instance;

    [SerializeField] public GameObject dialoguePanel;
    [SerializeField] public TextMeshProUGUI dialogueText;
    [SerializeField] private float typingSpeed = 0.03f;

    private string[] sentences;
    private int currentIndex;
    private Coroutine typingCoroutine;

    private bool isTyping;
    private bool isDialogueActive;

    private PlayerMovement playerMovement;
    private Action onDialogueCompleteCallback; // NEW

    public Action OnDialogueStart, OnDialogueEnd;
    public Action<int> OnSentenceChanged;


    private void Awake()
    {
        Instance = this;
        dialoguePanel.SetActive(false);
        playerMovement = FindObjectOfType<PlayerMovement>();
    }

    public void StartDialogue(string[] lines, Action onComplete = null)
    {
        if (isDialogueActive) return;

        sentences = lines;
        currentIndex = 0;
        isDialogueActive = true;
        onDialogueCompleteCallback = onComplete;

        OnDialogueStart?.Invoke();
        OnSentenceChanged?.Invoke(currentIndex);
        if (playerMovement != null) playerMovement.enabled = false;

        ShowPanelAndStartTyping();
    }

    private void ShowPanelAndStartTyping()
    {
        dialoguePanel.SetActive(true);
        dialoguePanel.transform.localScale = Vector3.zero;

        dialoguePanel.transform.DOScale(Vector3.one, 0.4f)
            .SetEase(Ease.OutBack)
            .OnComplete(() => StartTyping());
    }

    private void StartTyping()
    {
        if (typingCoroutine != null) StopCoroutine(typingCoroutine);
        typingCoroutine = StartCoroutine(TypeSentence(sentences[currentIndex]));
    }

    private IEnumerator TypeSentence(string sentence)
    {
        isTyping = true;
        dialogueText.text = "";

        foreach (char letter in sentence)
        {
            dialogueText.text += letter;
            yield return new WaitForSeconds(typingSpeed);
        }

        isTyping = false;
    }

    private void Update()
    {
        if (!isDialogueActive || !dialoguePanel.activeSelf) return;

        if (Input.GetMouseButtonDown(0))
        {
            if (isTyping)
            {
                StopCoroutine(typingCoroutine);
                dialogueText.text = sentences[currentIndex];
                isTyping = false;
            }
            else
            {
                currentIndex++;
                if (currentIndex < sentences.Length)
                {
                    OnSentenceChanged?.Invoke(currentIndex);
                    StartTyping();
                }
                else
                {
                    EndDialogue();
                }
            }
        }
    }

    private void EndDialogue()
    {
        dialoguePanel.transform.DOScale(Vector3.zero, 0.3f)
            .SetEase(Ease.InBack)
            .OnComplete(() =>
            {
                dialoguePanel.SetActive(false);
                isDialogueActive = false;
                dialogueText.text = ""; // Clear the text
                if (playerMovement != null) playerMovement.enabled = true;

                onDialogueCompleteCallback?.Invoke(); // <- callback executed here
                onDialogueCompleteCallback = null;
                OnDialogueEnd?.Invoke();
            });
    }

    public bool IsDialogueActive() => isDialogueActive;
}
