using UnityEngine;
using System.Collections;
public class JumpScareTrigger : MonoBehaviour
{
    public GameObject jumpscareModel;      // The model that appears for the scare
    public AudioSource bgmAudio;           // Background music AudioSource
    public AudioSource jumpscareAudio;     // Jumpscare sound AudioSource
    public float jumpscareDuration = 3f;     // How long the scare lasts

    // Effect references
    public MonoBehaviour playerScript;     // Drag your player movement script here
    public CameraShake cameraShake;        // Drag your Main Camera (with CameraShake script) here

    public void TriggerJumpscare()
    {
        StartCoroutine(JumpscareSequence());
    }

    IEnumerator JumpscareSequence()
    {
        // Freeze player movement
        if (playerScript != null)
            playerScript.enabled = false;

        // Shake the camera (shake for 0.5 seconds at 0.2 magnitude)
        if (cameraShake != null)
            StartCoroutine(cameraShake.Shake(0.5f, 0.2f));

        // Lower background music volume
        if (bgmAudio != null)
            bgmAudio.volume = 0.2f;

        // Show the jumpscare model
        if (jumpscareModel != null)
            jumpscareModel.SetActive(true);

        // Play jumpscare sound
        if (jumpscareAudio != null)
            jumpscareAudio.Play();

        // Wait for the jumpscare duration
        yield return new WaitForSeconds(jumpscareDuration);

        // Hide the jumpscare model
        if (jumpscareModel != null)
            jumpscareModel.SetActive(false);

        // Restore background music volume
        if (bgmAudio != null)
            bgmAudio.volume = 1f;

        // Unfreeze player movement
        if (playerScript != null)
            playerScript.enabled = true;
    }
}


/*
     public GameObject jumpscareModel;
    public AudioSource bgmAudio;
    public AudioSource jumpscareAudio;
    public float jumpscareDuration = 3f;

    public void TriggerJumpscare()
    {
        StartCoroutine(JumpscareSequence());
    }

    IEnumerator JumpscareSequence()
    {
        // Lower BGM volume
        bgmAudio.volume = 0.2f;

        // Show model
        jumpscareModel.SetActive(true);

        // Play jumpscare audio
        jumpscareAudio.Play();

        // Wait for duration
        yield return new WaitForSeconds(jumpscareDuration);

        // Hide model
        jumpscareModel.SetActive(false);

        // Restore BGM volume
        bgmAudio.volume = 1f;
    }
 */