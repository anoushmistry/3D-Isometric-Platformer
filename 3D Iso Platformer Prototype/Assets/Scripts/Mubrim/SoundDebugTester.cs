using UnityEngine;

public class SoundDebugTester : MonoBehaviour
{
    public AudioSource testSource;
    public AudioClip testClip;

    void Start()
    {
        if (testSource != null && testClip != null)
        {
            testSource.clip = testClip;
            testSource.loop = true;
            testSource.volume = 1f;
            testSource.Play();
            Debug.Log("Playing test sound.");
        }
        else
        {
            Debug.LogWarning("Missing testSource or testClip!");
        }
    }
}
