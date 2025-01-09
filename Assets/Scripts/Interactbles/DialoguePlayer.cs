using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DialoguePlayer : MonoBehaviour
{
    public AudioClip dialogueClip;
    public AudioSource audioSource;
    
    [SerializeField] private string subtitleText;

    [SerializeField] private int Index;
    public AudioController audiocontroller;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }

        if (dialogueClip != null)
            audioSource.clip = dialogueClip;
        else
            Debug.LogWarning("No dialogue clip assigned to this object.");

        audioSource.playOnAwake = false;
        audioSource.spatialBlend = 1.0f;
        audioSource.maxDistance = 10f;
    }

    public string InteractionMessage()
    {
        return "Press E to listen";
    }

    public void Interact()
    {
        if (!audioSource.isPlaying)
        {
            audiocontroller.PlayAudio(audioSource, Index, subtitleText);
        }
        else
            Debug.Log("Dialogue is already playing.");
    }

    // private System.Collections.IEnumerator releaseAudioBus()
    // {
    //     yield return new WaitForSeconds(audioSource.clip.length);
    //     audiocontroller.isAudioPlaying = false;
    //     subtitlePanel.SetActive(false);
    //     if (playerController != null)
    //     {
    //         playerController.enabled = true;
    //         gameUI.SetActive(true);
    //     }
    // }

}