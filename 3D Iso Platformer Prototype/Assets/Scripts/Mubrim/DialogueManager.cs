//Handles the display, typing effect, advancing through lines, and UI updates.
using System.Collections;
using UnityEngine;
using TMPro;
using DG.Tweening;

public class DialogueManager : MonoBehaviour
{
    public static DialogueManager Instance;

    [SerializeField] private GameObject dialoguePanel;
    [SerializeField] private TextMeshProUGUI dialogueText;
    [SerializeField] private float typingSpeed = 0.03f;

    private string[] sentences;
    private int currentIndex;
    private Coroutine typingCoroutine;

    private bool isTyping;
    private bool isDialogueActive;

    private PlayerMovement playerMovement;

    private void Awake()
    {
        Instance = this;
        dialoguePanel.SetActive(false);
        playerMovement = FindObjectOfType<PlayerMovement>();
    }

    public void StartDialogue(string[] lines)
    {
        if (isDialogueActive) return;

        sentences = lines;
        currentIndex = 0;
        isDialogueActive = true;

        // Lock player movement
        if (playerMovement != null) playerMovement.enabled = false;

        ShowPanelAndStartTyping();
    }

    private void ShowPanelAndStartTyping()
    {
        dialoguePanel.SetActive(true);
        dialoguePanel.transform.localScale = Vector3.zero;

        // Animate pop-in and then begin typing
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

                // Unlock player movement
                if (playerMovement != null) playerMovement.enabled = true;
            });
    }

    public bool IsDialogueActive() => isDialogueActive;
}
