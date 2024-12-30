using UnityEngine;

public class DialoguePlayer : MonoBehaviour, IInteractables
{
    public AudioClip dialogueClip;
    private AudioSource audioSource;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }

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
        if (dialogueClip != null && !audioSource.isPlaying)
        {
            audioSource.clip = dialogueClip;
            audioSource.Play();
            Debug.Log("Playing dialogue: " + dialogueClip.name);
        }
        else if (audioSource.isPlaying)
        {
            Debug.Log("Dialogue is already playing.");
        }
        else
        {
            Debug.LogWarning("No dialogue clip assigned to this object.");
        }
    }
}