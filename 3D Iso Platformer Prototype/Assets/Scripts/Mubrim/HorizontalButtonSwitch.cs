using UnityEngine;
using DG.Tweening;

public class HorizontalButtonSwitch : Interactable
{
    public Transform platform;               // The platform to move
    public Transform pointA;                 // Starting point
    public Transform pointB;                 // Target point
    public Transform switchButton;           // Visual switch button
    public float pressDepth = 0.1f;
    public float pressDuration = 0.2f;
    public float moveDuration = 2f;          // Duration to move the platform

    private Vector3 originalButtonPosition;
    private bool atPointA = true;

    private void Start()
    {
        originalButtonPosition = switchButton.localPosition;
    }

    public override void Interact()
    {
        AnimatePress();

        if (atPointA)
        {
            platform.DOMove(pointB.position, moveDuration).SetEase(Ease.InOutSine);
        }
        else
        {
            platform.DOMove(pointA.position, moveDuration).SetEase(Ease.InOutSine);
        }

        atPointA = !atPointA;
    }

    private void AnimatePress()
    {
        switchButton.DOLocalMoveY(originalButtonPosition.y - pressDepth, pressDuration / 2).SetEase(Ease.OutQuad).OnComplete(() =>
        {
            switchButton.DOLocalMoveY(originalButtonPosition.y, pressDuration / 2).SetEase(Ease.InQuad);
        });
    }
}
