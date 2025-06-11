using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    public static PlayerInventory Instance;
    public bool HasKey { get; private set; } = false;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }
    public void UseKey()
    {
        HasKey = false;
    }

    public void PickupKey()
    {
        HasKey = true;
    }
}
