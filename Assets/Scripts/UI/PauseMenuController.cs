using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenuController : MonoBehaviour
{
    public GameObject pauseMenuPanel;
    public GameObject subtitlePanel;
    public AudioSource[] audioSources;
    private bool isPaused = false;
    private bool subs = false;

    void Start()
    {
        pauseMenuPanel.SetActive(false);
        if (audioSources.Length == 0)
        {
            audioSources = FindObjectsOfType<AudioSource>();
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused)
            {
                ResumeGame();
            }
            else
            {
                PauseGame();
            }
        }
    }

    public void PauseGame()
    {
        isPaused = true;
        pauseMenuPanel.SetActive(true);
        subs = subtitlePanel.activeSelf;
        subtitlePanel.SetActive(false);
        Time.timeScale = 0f;

        foreach (AudioSource audio in audioSources)
        {
            audio.Pause();
        }
        
        Cursor.lockState = CursorLockMode.None; // Unlocks the cursor
        Cursor.visible = true;                  // Makes the cursor visible
    }

    public void ResumeGame()
    {
        isPaused = false;
        pauseMenuPanel.SetActive(false);
        subtitlePanel.SetActive(subs);
        Time.timeScale = 1f; 
        
        foreach (AudioSource audio in audioSources)
        {
            audio.UnPause();
        }
        
        Cursor.lockState = CursorLockMode.Locked; // Locks the cursor again for gameplay
        Cursor.visible = false;                   // Hides the cursor
    }

    public void QuitToMainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu");
    }

    public void QuitGame()
    {
        Time.timeScale = 1f;
        Application.Quit();
        Debug.Log("Game Quit!");
    }
}
