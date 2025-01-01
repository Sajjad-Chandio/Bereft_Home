using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Video;
using UnityEngine.SceneManagement;

public class ChoiceSystem : MonoBehaviour
{
    private const int ELEM_SIZE = 8;

    [Header("UI Elements")]
    public GameObject choiceUI;
    public GameObject gameUI;
    public Button journalButton;
    public Button gunButton;
    public TextMeshProUGUI objectiveText;

    [Header("Audio n Video")]
    public AudioClip dialogueClip;
    private AudioSource audioSource;
    public GameObject videoPanel; 
    public VideoPlayer videoPlayer;
    public VideoClip journalVideoClip; 
    public VideoClip gunVideoClip;   

    [Header("Player Movement")]
    public RealisticFirstPersonController playerController;

    private bool isChoiceActive = false;

    [Header("Variables")]
    public bool[] isChecked;
    public int countCheck;
    public bool isAudioPlaying;

    private void Start()
    {
        choiceUI.SetActive(false);
        videoPanel.SetActive(false);
        audioSource = GetComponent<AudioSource>();

        journalButton.onClick.AddListener(() => PlayVideo(journalVideoClip));
        gunButton.onClick.AddListener(() => PlayVideo(gunVideoClip));

        isChecked = new bool[ELEM_SIZE];
        for (int i = 0; i < isChecked.Length; i++)
        {
            isChecked[i] = false;
        }
        countCheck = 0;

        UpdateObjectiveText();
    }

    public void setterIsChecked(int i)
    {
        if (!(countCheck == isChecked.Length) && !isChecked[i])
        {
            isChecked[i] = true;
            countCheck++;
            UpdateObjectiveText(); // Update UI whenever an objective is completed

            if (countCheck == isChecked.Length)
                unlockZone();
        }
    }

    // run when objective done
    private void unlockZone()
    {
        Collider collider = GetComponent<Collider>();
        if (collider != null)
            collider.isTrigger = true; // Set the collider as a trigger
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!isAudioPlaying && !isChoiceActive && other.CompareTag("Player"))
            TriggerChoice();
    }

    private void TriggerChoice()
    {
        isChoiceActive = true;
        choiceUI.SetActive(true);
        gameUI.SetActive(false);

        audioSource.clip = dialogueClip;
        audioSource.Play();
        isAudioPlaying = true;
        StartCoroutine(releaseAudioBus());
        Debug.Log("Playing dialogue: " + dialogueClip.name);

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

    private System.Collections.IEnumerator releaseAudioBus()
    {
        yield return new WaitForSeconds(audioSource.clip.length);
        isAudioPlaying = false;
    }

    private void PlayVideo(VideoClip videoClip)
    {
        if(isAudioPlaying)
        {
            Debug.LogWarning("Dialogue playing");
            return;
        }
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
        SceneManager.LoadScene("EndLevel");

        // videoPanel.SetActive(false); // Hide the video panel after video finishes
        // gameUI.SetActive(true);

        // if (playerController != null)
        // {
        //     playerController.enabled = true;
        //     Cursor.lockState = CursorLockMode.Locked; // Locks the cursor again for gameplay
        //     Cursor.visible = false;                   // Hides the cursor
        // }

        // isChoiceActive = false;

    }

    private void UpdateObjectiveText()
    {
        objectiveText.text = countCheck < isChecked.Length ?
        $"{countCheck}/{isChecked.Length} Objects Found\nListen to more objects" :
        $"All objects found\nPlease go to bedroom" ;
    }
}

