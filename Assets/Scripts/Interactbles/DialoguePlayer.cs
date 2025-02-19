using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DialoguePlayer : MonoBehaviour
{
    public AudioClip dialogueClip;
    public GameObject gameUI;
    private AudioSource audioSource;
    
    public GameObject subtitlePanel;
    public string subtitleText;
    private TextMeshProUGUI subtitle;

    public int Index;
    public ChoiceSystem audiocontroller;
    public RealisticFirstPersonController playerController;

    void Start()
    {
        // audiocontroller = ChoiceSystem.GetComponent<ChoiceSystem>();
        gameUI.SetActive(true);

        subtitle = subtitlePanel.GetComponent<TextMeshProUGUI>();
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

            subtitle.text = subtitleText;
            subtitlePanel.SetActive(true);

            audiocontroller.setterIsChecked(Index);
            if (playerController != null)
            {
                playerController.enabled = false;
                gameUI.SetActive(false);
            }

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
        subtitlePanel.SetActive(false);
        if (playerController != null)
        {
            playerController.enabled = true;
            gameUI.SetActive(true);
        }
    }

}