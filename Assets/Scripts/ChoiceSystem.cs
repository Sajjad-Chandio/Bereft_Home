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

    
    public bool[] isChecked;
    public bool allChecked;

    public void setterIsChecked(int i)
    {
        if (!allChecked && !isChecked[i])
        {
            isChecked[i] = true;
            allChecked = true;
            for (int j = 0; j < 8; j++)
            {
                if (!isChecked[j])
                    allChecked = false;
            }
            if (allChecked)
                unlockZone();
        }
    }

    private void Start()
    {
        choiceUI.SetActive(false);

        journalButton.onClick.AddListener(() => ChooseOption(journalClip));
        gunButton.onClick.AddListener(() => ChooseOption(gunClip));

        isChecked = new bool[8];
        for (int i = 0; i < 8; i++)
        {
            isChecked[i] = false;
        }
        allChecked = false;
    }

    // run when objective done
    private void unlockZone()
    {
        Collider collider = GetComponent<Collider>();
        if (collider != null)
        {
            collider.isTrigger = true;  // Set the collider as a trigger
        }
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
            Cursor.lockState = CursorLockMode.None; // Unlocks the cursor
            Cursor.visible = true;                  // Makes the cursor visible
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
            Cursor.lockState = CursorLockMode.Locked; // Locks the cursor again for gameplay
            Cursor.visible = false;                   // Hides the cursor
        }

        isChoiceActive = false;
    }

}
