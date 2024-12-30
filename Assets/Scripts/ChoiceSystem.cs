using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Video;

public class ChoiceSystem : MonoBehaviour
{
    [Header("UI Elements")]
    public GameObject choiceUI;
    public Button journalButton;
    public Button gunButton;
    public TextMeshProUGUI objectiveText;

    [Header("Video")]
    public GameObject videoPanel; 
    public VideoPlayer videoPlayer;
    public VideoClip journalVideoClip; 
    public VideoClip gunVideoClip;   

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

            UpdateObjectiveText(); // Update UI whenever an objective is completed

            if (allChecked)
                unlockZone();
        }
    }

    private void Start()
    {
        choiceUI.SetActive(false);
        videoPanel.SetActive(false);

        journalButton.onClick.AddListener(() => PlayVideo(journalVideoClip));
        gunButton.onClick.AddListener(() => PlayVideo(gunVideoClip));

        isChecked = new bool[8];
        for (int i = 0; i < 8; i++)
        {
            isChecked[i] = false;
        }
        allChecked = false;

        UpdateObjectiveText();
    }

    // run when objective done
    private void unlockZone()
    {
        Collider collider = GetComponent<Collider>();
        if (collider != null)
        {
            collider.isTrigger = true; // Set the collider as a trigger
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

    private void PlayVideo(VideoClip videoClip)
    {
        if (videoClip != null)
        {
            videoPanel.SetActive(true); // Show the video panel
            videoPlayer.clip = videoClip;
            videoPlayer.Play();
        }
        else
        {
            Debug.LogWarning("Video clip is missing.");
        }

        choiceUI.SetActive(false);

        StartCoroutine(WaitForVideoToFinish());
    }

    private System.Collections.IEnumerator WaitForVideoToFinish()
    {
        while (videoPlayer.isPlaying)
        {
            yield return null;
        }

        videoPanel.SetActive(false); // Hide the video panel after video finishes

        if (playerController != null)
        {
            playerController.enabled = true;
            Cursor.lockState = CursorLockMode.Locked; // Locks the cursor again for gameplay
            Cursor.visible = false;                   // Hides the cursor
        }

        isChoiceActive = false;
    }

    private void UpdateObjectiveText()
    {
        int completedCount = 0;

        foreach (bool isComplete in isChecked)
        {
            if (isComplete) completedCount++;
        }

        objectiveText.text = $"{completedCount}/{isChecked.Length} Objectives Completed";
    }
}
