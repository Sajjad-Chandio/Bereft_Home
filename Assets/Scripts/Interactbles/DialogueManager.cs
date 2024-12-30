using UnityEngine;

public class DialogueManager : MonoBehaviour
{
    // Singleton pattern for easy access
    public static DialogueManager Instance { get; private set; }

    // Public property to check if dialogue is currently playing
    public bool IsDialoguePlaying { get; private set; }

    private RealisticFirstPersonController playerController;

    void Awake()
    {
        // Ensure only one instance exists
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        // Find the player controller in the scene
        playerController = FindObjectOfType<RealisticFirstPersonController>();
    }

    // Called when dialogue starts playing
    public void StartDialogue()
    {
        IsDialoguePlaying = true;
        if (playerController != null)
        {
            playerController.enabled = false;
        }
    }

    // Called when dialogue finishes playing
    public void EndDialogue()
    {
        IsDialoguePlaying = false;
        if (playerController != null)
        {
            playerController.enabled = true;
        }
    }
}
