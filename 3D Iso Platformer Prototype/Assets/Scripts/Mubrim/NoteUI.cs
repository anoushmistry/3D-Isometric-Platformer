using UnityEngine;
using TMPro;
using DG.Tweening;

public class NoteUI : MonoBehaviour
{
    public static NoteUI Instance;

    [SerializeField] private GameObject panel;
    [SerializeField] private TextMeshProUGUI noteText;

    private void Awake()
    {
        Instance = this;
        panel.SetActive(false);
        panel.transform.localScale = Vector3.zero;
    }

    public void ShowNote(string text)
    {
        panel.SetActive(true);
        panel.transform.DOKill();
        panel.transform.localScale = Vector3.zero;
        noteText.text = text;
        panel.transform.DOScale(Vector3.one, 0.5f).SetEase(Ease.OutBack);

        var playerMovement = FindObjectOfType<PlayerMovement>();
        if (playerMovement != null)
            playerMovement.LockInput = true;
    }


    public void HideNote()
    {
        panel.transform.DOScale(Vector3.zero, 0.2f).SetEase(Ease.InBack).OnComplete(() =>
        {
            panel.SetActive(false);
            noteText.text = "";

            var playerMovement = FindObjectOfType<PlayerMovement>();
            if (playerMovement != null)
                playerMovement.LockInput = false;
        });
    }
}
