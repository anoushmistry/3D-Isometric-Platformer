using UnityEngine;

public class KeyManager : MonoBehaviour
{
    public static KeyManager Instance;

    [SerializeField] private GameObject keyIcon;

    private bool hasKey = false;

    private void Awake()
    {
        if (Instance != null) Destroy(gameObject);
        Instance = this;
    }

    public void PickupKey()
    {
        hasKey = true;
        if (keyIcon != null) keyIcon.SetActive(true);
    }

    public bool HasKey() => hasKey;
}
