using UnityEngine;

public class ShaderDoorController : MonoBehaviour
{
    public PlayerInteraction playerInteraction; // Assign in Inspector
    private Animator m_animator;
    private int m_animIDOpen;

    private void Start()
    {
        m_animator = GetComponent<Animator>();
        m_animIDOpen = Animator.StringToHash("Open");
    }

    private void Update()
    {
        if (Input.GetButtonDown("Submit"))
        {
            if (playerInteraction != null && playerInteraction.HasPlacedOrb())
            {
                m_animator.SetTrigger(m_animIDOpen);
                Debug.Log("Door Opened!");
            }
            else
            {
                Debug.Log("Door Locked! Place the Orb first.");
            }
        }
    }
}
