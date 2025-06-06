using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class AudioController : MonoBehaviour
{
    [Header("UI Elements")]
    public GameObject gameUI;
    public GameObject subtitlePanel;

    [Header("Player Control")]
    public RealisticFirstPersonController playerController;

    [Header("Start Narration")]
    public AudioSource playerAudioSource;
    public AudioClip startClip;
    [TextArea] public string startSubtitle; 

    public ChoiceSystem objective;

    private TextMeshProUGUI subtitle;
    private bool isAudioPlaying;

    void Start()
    {
       
        gameUI.SetActive(true);
        subtitle = subtitlePanel.GetComponent<TextMeshProUGUI>();
        subtitlePanel.SetActive(false);

       
        if (playerAudioSource == null)
        {
            Debug.LogError("Player AudioSource is not assigned.");
            return;
        }
        
        playerAudioSource.Stop();

        if (startClip != null)
        {
            playerAudioSource.clip = startClip;
            PlayAudio(playerAudioSource, -1, startSubtitle);
        }
        else
        {
            Debug.LogWarning("No dialogue clip assigned to starting narration.");
        }
    }

    public void PlayAudio(AudioSource audioPlayer, int index, string subtitleText)
    {
        if (isAudioPlaying)
        {
            Debug.Log("Dialogue is already playing.");
            return;
        }
        
        isAudioPlaying = true;
        audioPlayer.Play();
        Debug.Log("Playing dialogue: " + audioPlayer.clip.name);
        if (index != -1)
            objective.setterIsChecked(index, audioPlayer);

        subtitle.text = subtitleText;
        subtitlePanel.SetActive(true);

        StartCoroutine(HandleAudioCompletion(audioPlayer));
    }

    private System.Collections.IEnumerator HandleAudioCompletion(AudioSource audioPlayer)
    {
        yield return new WaitForSeconds(audioPlayer.clip.length);

        isAudioPlaying = false;
        subtitlePanel.SetActive(false);
    }
}
