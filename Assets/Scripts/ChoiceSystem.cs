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
    public GameObject subtitlePanel;

    [Header("Audio n Video")]
    public AudioClip dialogueClip;
    private AudioSource audioSource;
    public GameObject videoPanel; 
    public VideoPlayer videoPlayer;

    public VideoClip journalVideoClip; 
    public VideoClip gunVideoClip;   

    // private string journalVideoURL;
    // private string gunVideoURL;

    [Header("Player Movement")]
    public RealisticFirstPersonController playerController;

    // private bool isChoiceActive = false;

    [Header("Variables")]
    public bool[] isChecked;
    public int countCheck;
    // public bool isAudioPlaying;

    private void Start()
    {
        choiceUI.SetActive(false);
        videoPanel.SetActive(false);
        subtitlePanel.SetActive(false);
        audioSource = GetComponent<AudioSource>();

        // journalVideoURL = Application.streamingAssetsPath + "/If the Player Chooses the Journal.mp4";
        // gunVideoURL = Application.streamingAssetsPath + "/If the Player Chooses the Gun.mp4";

        // journalButton.onClick.AddListener(() => PlayVideo(journalVideoURL));
        // gunButton.onClick.AddListener(() => PlayVideo(gunVideoURL));

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

    public void setterIsChecked(int i, AudioSource audioSource)
    {
        if (!(countCheck == isChecked.Length) && !isChecked[i])
        {
            isChecked[i] = true;
            countCheck++;
            UpdateObjectiveText(); // Update UI whenever an objective is completed

            if (countCheck == isChecked.Length)
                StartCoroutine(unlockZone(audioSource));
        }
    }

    // run when objective done
    private System.Collections.IEnumerator unlockZone(AudioSource audioSource)
    {
        yield return new WaitForSeconds(audioSource.clip.length);

        Collider collider = GetComponent<Collider>();
        if (collider != null)
            collider.isTrigger = true; // Set the collider as a trigger
    }

    private void OnTriggerEnter(Collider other)
    {
        if (/*!isChoiceActive && */other.CompareTag("Player"))
            TriggerChoice();
    }

    private void TriggerChoice()
    {
        // isChoiceActive = true;
        choiceUI.SetActive(true);
        gameUI.SetActive(false);

        audioSource.clip = dialogueClip;
        audioSource.Play();
        subtitlePanel.SetActive(true);
        // isAudioPlaying = true;
        StartCoroutine(releaseAudioBus());
        Debug.Log("Playing dialogue: " + dialogueClip.name);

        journalButton.interactable = false;
        gunButton.interactable = false;

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
        // isAudioPlaying = false;
        
        journalButton.interactable = true;
        gunButton.interactable = true;
        subtitlePanel.SetActive(false);
    }

    private void PlayVideo(/*string*/VideoClip videoClip)
    {
        // if(isAudioPlaying)
        // {
        //     Debug.LogWarning("Dialogue playing");
        //     return;
        // }
        if (videoClip != null)
        {
            videoPanel.SetActive(true); // Show the video panel
            videoPlayer.clip/*url*/ = videoClip;
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
    }

    private void UpdateObjectiveText()
    {
        objectiveText.text = countCheck < isChecked.Length ?
        $"{countCheck}/{isChecked.Length} Objects Found\nListen to more objects" :
        $"All objects found\nPlease go to bedroom" ;
    }
}

