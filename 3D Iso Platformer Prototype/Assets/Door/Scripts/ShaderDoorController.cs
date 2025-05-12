using UnityEngine;

public class ShaderDoorController : MonoBehaviour
{
    public PlayerController PlayerController;
    public Transform m_hinge;

    private Animator m_animator;
    private int m_animIDOpen;

    void Start()
    {
        m_animator = GetComponent<Animator>();
        m_animIDOpen = Animator.StringToHash("Open");
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))        //For now
        {
            if (OrbPlacementManager.orbPlaced)  // Only open if orb is placed
            {
                m_animator.SetTrigger(m_animIDOpen);
                //PlayerController.SetMaterialStencil();
            }
            else
            {
                Debug.Log("You must place the Light Orb first to open the door!");
            }
        }
    }
}
