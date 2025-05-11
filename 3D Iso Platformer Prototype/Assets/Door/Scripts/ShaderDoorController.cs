using System.Collections;
using System.Collections.Generic;
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
        if (Input.GetButtonDown("Submit"))
        {
            m_animator.SetTrigger(m_animIDOpen);
            //PlayerController.SetMaterialStencil();
        }
    }
}
