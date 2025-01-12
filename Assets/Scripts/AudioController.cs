using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class AudioController : MonoBehaviour
{
    public GameObject gameUI;
    public GameObject subtitlePanel;
    private TextMeshProUGUI subtitle;
    private bool isAudioPlaying;

    [Header("Player Control")]
    public RealisticFirstPersonController playerController;

    [Header("Start Narration")]
    public AudioSource playerAudioSource;
    public AudioClip startClip;
    public string startSubtitle;

    public ChoiceSystem objective;
    

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        gameUI.SetActive(true);
        subtitle = subtitlePanel.GetComponent<TextMeshProUGUI>();
        subtitlePanel.SetActive(false);
        playerAudioSource.Stop();

        if (startClip != null)
        {
            playerAudioSource.clip = startClip;
            PlayAudio(playerAudioSource, -1, startSubtitle);
        }
        else
            Debug.LogWarning("No dialogue clip assigned to starting narration.");
    }
    

    public void PlayAudio(AudioSource audioPlayer, int Index, string subtitleText)
    {
        if (!isAudioPlaying)
        {
            isAudioPlaying = true;
            audioPlayer.Play();
            Debug.Log("Playing dialogue: " + audioPlayer.clip.name);

            subtitle.text = subtitleText;
            subtitlePanel.SetActive(true);

            if(Index != -1)
                objective.setterIsChecked(Index);

            // if (playerController != null)
            // {
            //     playerController.enabled = false;
            // }

            // gameUI.SetActive(false);

            StartCoroutine(releaseAudioBus(audioPlayer));
        }
        else
            Debug.Log("Dialogue is already playing.");
    }

    private System.Collections.IEnumerator releaseAudioBus(AudioSource audioPlayer)
    {
        yield return new WaitForSeconds(audioPlayer.clip.length);
        isAudioPlaying = false;
        subtitlePanel.SetActive(false);
        // gameUI.SetActive(true);
        // playerController.enabled = true;
    }

    // Update is called once per frame
    // void Update()
    // {
        
    // }
}
