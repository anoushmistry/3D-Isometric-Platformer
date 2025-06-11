using UnityEngine;
using DG.Tweening;
using System.Collections;

public class FloatingBridge : MonoBehaviour
{
    public GameObject[] bridgePieces;
    public float delayBetweenPieces = 0.25f;
    public float scaleDuration = 0.5f;
    public float floatHeight = 0.5f;
    public float rotationAmount = 360f;
    public Vector3 originalScale;

    public GameObject smokeParticlePrefab;

    public void ActivateBridge()
    {
        StartCoroutine(AnimateBridgePieces());
    }

    private IEnumerator AnimateBridgePieces()
    {
        foreach (GameObject piece in bridgePieces)
        {
            originalScale = piece.transform.localScale;
            piece.SetActive(true);
            piece.transform.localScale = Vector3.zero;

            Vector3 originalPos = piece.transform.position;
            piece.transform.position = originalPos + Vector3.up * floatHeight;

            piece.transform.DOMove(originalPos, scaleDuration).SetEase(Ease.OutBounce);
            piece.transform.DOScale(originalScale, scaleDuration).SetEase(Ease.OutBack);
            piece.transform.DORotate(new Vector3(0, 0, rotationAmount), scaleDuration, RotateMode.FastBeyond360).SetEase(Ease.OutCubic);

            // 🌫️ Instantiate smoke around the piece, slightly above ground
            if (smokeParticlePrefab != null)
            {
                Vector3 particlePos = originalPos + new Vector3(0, 0.25f, 0); // Slightly up
                GameObject smoke = Instantiate(smokeParticlePrefab, particlePos, Quaternion.identity);
                smoke.transform.localScale = Vector3.one * 1.5f; // Scale up smoke size if needed
                Destroy(smoke, 2f);
            }

            yield return new WaitForSeconds(delayBetweenPieces);
        }
    }
}
