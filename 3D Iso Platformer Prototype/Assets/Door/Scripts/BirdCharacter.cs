using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BirdCharacter : MonoBehaviour
{
    public float speed;
    public float rotationSpeed;
    public float animationBlendSpeed;

    private float m_currentRotationalVelocity;
    private float m_targetRotation;
    private float m_animationBlend;
    private int m_animIDSpeed;

    private GameObject m_mainCamera;
    private CharacterController m_characterController;
    private Animator m_animator;
    private SkinnedMeshRenderer m_skin;

    private void Awake()
    {
        m_mainCamera = GameObject.FindGameObjectWithTag("MainCamera");
    }

    void Start()
    {
        m_characterController = GetComponent<CharacterController>();    
        m_animator = GetComponent<Animator>();
        m_animIDSpeed = Animator.StringToHash("Speed");
        m_skin = GetComponentInChildren<SkinnedMeshRenderer>();
    }

    void Update()
    {
        DoMovement();
    }

    private void DoMovement()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");
        float inputMagnitude = Vector2.SqrMagnitude(new Vector2(horizontal, vertical));
        inputMagnitude = inputMagnitude < 0.1f ? 0.0f : inputMagnitude;

        if (inputMagnitude > 0.0f)
        {
            m_targetRotation = Mathf.Atan2(horizontal, vertical) * Mathf.Rad2Deg + m_mainCamera.transform.eulerAngles.y;
            float rotation = Mathf.SmoothDampAngle(transform.eulerAngles.y, m_targetRotation, ref m_currentRotationalVelocity, rotationSpeed);
            transform.rotation = Quaternion.Euler(0.0f, rotation, 0.0f);
        }

        Vector3 targetDirection = Quaternion.Euler(0.0f, m_targetRotation, 0.0f) * Vector3.forward;

        m_characterController.Move(targetDirection.normalized * (speed * Time.deltaTime) * inputMagnitude);

        m_animationBlend = Mathf.Lerp(m_animationBlend, inputMagnitude * speed, Time.deltaTime * animationBlendSpeed);
        m_animator.SetFloat(m_animIDSpeed, m_animationBlend);
    }

    public void SetMaterialStencil()
    {
        foreach (Material mat in m_skin.materials)
        {
            mat.SetFloat("_StencilRef", 1);
            mat.SetFloat("_StencilComp", 4);
            mat.SetFloat("_ShadowClipValue", 0.3f);
        }
    }
}
