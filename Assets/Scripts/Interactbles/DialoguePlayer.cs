using UnityEngine;

public class DialoguePlayer : MonoBehaviour, IInteractables
{
    public AudioClip dialogueClip;
    private audiocontroller audiocontroller;
    private AudioSource audioSource;

    public int Index;
    private ChoiceSystem choice;

    void Start()
    {
        audiocontroller = GameObject.Find("AudioControl").GetComponent<audiocontroller>();
        choice = GameObject.Find("ChoiceSystem").GetComponent<ChoiceSystem>();

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
        if (dialogueClip != null && !audiocontroller.isAudioPlaying && !audioSource.isPlaying)
        {
            audiocontroller.isAudioPlaying = true;
            audioSource.clip = dialogueClip;
            audioSource.Play();
            Debug.Log("Playing dialogue: " + dialogueClip.name);

            choice.setterIsChecked(Index);

            StartCoroutine(releaseAudioBus());
        }
        else if (audiocontroller.isAudioPlaying || audioSource.isPlaying)
        {
            Debug.Log("Dialogue is already playing.");
        }
        else
        {
            Debug.LogWarning("No dialogue clip assigned to this object.");
        }
    }

    private System.Collections.IEnumerator releaseAudioBus()
    {
        yield return new WaitForSeconds(audioSource.clip.length);
        audiocontroller.isAudioPlaying = false;
    }

}