using UnityEngine;
using UnityEngine.UI;

public class ChoiceSystem : MonoBehaviour
{
    [Header("UI Elements")]
    public GameObject choiceUI;
    public Button journalButton;
    public Button gunButton;

    [Header("Audio")]
    public AudioSource dialogAudioSource;
    public AudioClip dialogClip;
    public AudioClip journalClip;
    public AudioClip gunClip;

    [Header("Player Movement")]
    public RealisticFirstPersonController playerController;

    private bool isChoiceActive = false;

    private void Start()
    {
        choiceUI.SetActive(false);

        journalButton.onClick.AddListener(() => ChooseOption(journalClip));
        gunButton.onClick.AddListener(() => ChooseOption(gunClip));
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !isChoiceActive)
        {
            TriggerChoice();
        }
    }

    private void TriggerChoice()
    {
        isChoiceActive = true;
        choiceUI.SetActive(true);

        if (dialogAudioSource != null && dialogClip != null)
        {
            dialogAudioSource.clip = dialogClip;
            dialogAudioSource.Play();
        }
        else
        {
            Debug.LogWarning("Dialog audio source or clip is missing.");
        }

        if (playerController != null)
        {
            playerController.enabled = false;
        }
        else
        {
            Debug.LogWarning("PlayerController is not assigned.");
        }
    }

    private void ChooseOption(AudioClip choiceClip)
    {
        if (dialogAudioSource != null && choiceClip != null)
        {
            dialogAudioSource.clip = choiceClip;
            dialogAudioSource.Play();
        }
        else
        {
            Debug.LogWarning("Choice audio source or clip is missing.");
        }

        choiceUI.SetActive(false);

        StartCoroutine(ReEnableMovementAfterAudio());
    }

    private System.Collections.IEnumerator ReEnableMovementAfterAudio()
    {
        yield return new WaitForSeconds(dialogAudioSource.clip.length);

        if (playerController != null)
        {
            playerController.enabled = true;
        }

        isChoiceActive = false;
    }
}
