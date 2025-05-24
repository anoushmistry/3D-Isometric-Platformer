using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    private GameObject player;
    private MeshRenderer m_skin;
    private Collider playerCollider;
    [SerializeField] private Camera mainCam;

    private const string RoomLayer = "Room";
    private const string PlayerLayer = "Player";
    private const string PlayerCameraLayer = "PlayerCamera";
    private const string PushableBoxLayer = "PushableBox"; 

    
    private Coroutine fadeCoroutine;

    public float interactDistance = 1f;
    public KeyCode pushKey = KeyCode.E;
    private PushableBox currentBox;

    [Header("Room Interaction Settings")]
    public Image fadeImage;
    [SerializeField] private float fadeDuration = 1.0f;
    private bool isFading = false;

    private void Start()
    {
        m_skin = GetComponent<MeshRenderer>();
        player = this.gameObject;
        playerCollider = player.GetComponent<Collider>();
    }

    void Update()
    {
        if (Input.GetKeyDown(pushKey))
        {
            if (currentBox == null)
            {
                Ray ray = new Ray(transform.position, transform.forward);
                if (Physics.Raycast(ray, out RaycastHit hit, interactDistance,LayerMask.GetMask("PushableBox")))
                {
                    PushableBox box = hit.collider.GetComponent<PushableBox>();
                    if (box != null)
                    {
                        box.StartPush(transform);
                        currentBox = box;
                    }
                }
            }
            else
            {
                currentBox.StopPush();
                currentBox = null;
            }
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Room"))
        {
            Debug.Log("Entered a Room");
            mainCam.cullingMask = LayerMask.GetMask(RoomLayer, PlayerLayer, PlayerCameraLayer, PushableBoxLayer);
            mainCam.clearFlags = CameraClearFlags.SolidColor;

            StartCoroutine(FadeOutThenFadeIn(0.01f,0.2f));
           
            //fadeCoroutine = StartCoroutine(FadeBackgroundColor(mainCam.backgroundColor, Color.black, fadeDuration));
            //mainCam.backgroundColor = Color.black; // Set the background color to white

        }
        if(other.CompareTag("CameraChange"))
        {
            gameObject.GetComponent<PlayerMovement>().SetCameraAngle(0);
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Room"))
        {
            StartCoroutine(FadeOutThenFadeIn(0.01f, 0.2f));


            mainCam.clearFlags = CameraClearFlags.Skybox;
            mainCam.cullingMask = ~(1 << LayerMask.NameToLayer(RoomLayer)); // Render all layers except "Room"
            //mainCam.cullingMask = LayerMask.GetMask(EverythingLayer);
        }

    }
    
    public IEnumerator FadeIn(float time)
    {
        yield return Fade(1, 0, time);
    }

    public IEnumerator FadeOut(float time)
    {
        yield return Fade(0, 1,time);
    }
    public IEnumerator FadeOutThenFadeIn(float fadeOutTime, float fadeInTime)
    {
        yield return StartCoroutine(FadeOut(fadeOutTime));
        yield return StartCoroutine(FadeIn(fadeInTime));
    }

    private IEnumerator Fade(float startAlpha, float endAlpha, float time)
    {
        if (isFading)
            yield break;

        isFading = true;
        float elapsed = 0f;
        Color color = fadeImage.color;

        while (elapsed < time)
        {
            float alpha = Mathf.Lerp(startAlpha, endAlpha, elapsed / time);
            fadeImage.color = new Color(color.r, color.g, color.b, alpha);
            elapsed += Time.deltaTime;
            yield return null;
        }

        // Ensure final alpha
        fadeImage.color = new Color(color.r, color.g, color.b, endAlpha);
        isFading = false;

    }
    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Vector3 origin = transform.position + new Vector3(0,1f,0);
        Vector3 direction = transform.forward * interactDistance;
        Gizmos.DrawRay(origin, direction);
    }
    public void SetMaterialStencil()        //Changing the stencil
    {
        foreach (Material mat in m_skin.materials)
        {
            //mat.SetFloat("_StencilRef", 1);
            ///mat.SetFloat("_StencilComp", 4);
           // mat.SetFloat("_ShadowClipValue", 0.3f);
        }

        //m_skin.shadowCastingMode = ShadowCastingMode.Off;
    }
    public void ResetMaterialStencil()      //Resetting the stencil
    {
        foreach (Material mat in m_skin.materials)
        {
            mat.SetFloat("_StencilRef", 0);
            mat.SetFloat("_StencilComp", 8);
            mat.SetFloat("_ShadowClipValue", 0.5f);
        }
    }
}
