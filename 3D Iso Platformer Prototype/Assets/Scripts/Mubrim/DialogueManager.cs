//Handles the display, typing effect, advancing through lines, and UI updates.
using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using DG.Tweening; // Add this only if using DOTween

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

    private void Awake()
    {
        Instance = this;
        dialoguePanel.SetActive(false);
    }

    public void StartDialogue(string[] lines)
    {
        sentences = lines;
        currentIndex = 0;

        ShowPanel(); // Enable and animate panel
        StartTyping();
    }

    private void ShowPanel()
    {
        dialoguePanel.SetActive(true);

        // Reset scale and animate pop-in
        dialoguePanel.transform.localScale = Vector3.zero;
        dialoguePanel.transform.DOScale(Vector3.one, 0.4f).SetEase(Ease.OutBack); // DOTween pop effect
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
        if (dialoguePanel.activeSelf && Input.GetMouseButtonDown(0))
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
                    HidePanel();
                }
            }
        }
    }

    private void HidePanel()
    {
        dialoguePanel.transform.DOScale(Vector3.zero, 0.3f).SetEase(Ease.InBack)
            .OnComplete(() => dialoguePanel.SetActive(false));
    }
}
