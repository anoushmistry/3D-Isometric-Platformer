using UnityEngine;
using UnityEngine.UI;

public class KeyUIController : MonoBehaviour
{
    public static KeyUIController Instance;

    [SerializeField] private Image keyIconImage;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);

        keyIconImage.gameObject.SetActive(false); 
    }

    public void ShowKeyIcon()
    {
        keyIconImage.gameObject.SetActive(true);
    }

    public void HideKeyIcon()
    {
        keyIconImage.gameObject.SetActive(false);
    }
}
