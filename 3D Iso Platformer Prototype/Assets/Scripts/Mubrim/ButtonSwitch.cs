using UnityEngine;
using DG.Tweening;

public class ButtonSwitch : Interactable
{
    public PlatformController platformController;  // Reference to PlatformController
    public bool isGroundSwitch;   // True if this switch is on ground, false if on platform
    public Transform switchButton; // The button part of switch to animate press
    public float pressDepth = 0.1f; // How far the button presses down
    public float pressDuration = 0.2f;

    private Vector3 originalButtonPosition;

    private void Start()
    {
        originalButtonPosition = switchButton.localPosition;
    }

    public override void Interact()
    {
        // Play press animation
        AnimatePress();

        if (isGroundSwitch)
        {
            // If platform is up, pressing ground switch moves it down
            if (platformController.IsPlatformUp())
            {
                platformController.MovePlatformDown();
            }
        }
        else
        {
            // If platform is down, pressing platform switch moves it up
            if (!platformController.IsPlatformUp())
            {
                platformController.MovePlatformUp();
            }
        }
    }

    private void AnimatePress()
    {
        // Press button down and up using DOTween
        switchButton.DOLocalMoveY(originalButtonPosition.y - pressDepth, pressDuration / 2).SetEase(Ease.OutQuad).OnComplete(() =>
        {
            switchButton.DOLocalMoveY(originalButtonPosition.y, pressDuration / 2).SetEase(Ease.InQuad);
        });
    }
}
