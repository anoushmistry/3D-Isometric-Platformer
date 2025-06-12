using UnityEngine;
using DG.Tweening;

public class ButtonSwitch : Interactable
{
    public PlatformController platformController;
    public bool isGroundSwitch;
    public Transform switchButton;
    public float pressDepth = 0.1f;
    public float pressDuration = 0.2f;

    private Vector3 originalButtonPosition;

    private void Start()
    {
        originalButtonPosition = switchButton.localPosition;
    }

    public override void Interact()
    {
        AnimatePress();

        // 🔊 Play lever sound
        if (SoundManager.Instance != null)
            SoundManager.Instance.PlayLeverSFX(transform.position);

        if (isGroundSwitch)
        {
            if (platformController.IsPlatformUp())
            {
                platformController.MovePlatformDown();
            }
        }
        else
        {
            if (!platformController.IsPlatformUp())
            {
                platformController.MovePlatformUp();
            }
        }
    }

    private void AnimatePress()
    {
        switchButton.DOLocalMoveY(originalButtonPosition.y - pressDepth, pressDuration / 2).SetEase(Ease.OutQuad).OnComplete(() =>
        {
            switchButton.DOLocalMoveY(originalButtonPosition.y, pressDuration / 2).SetEase(Ease.InQuad);
        });
    }
}
