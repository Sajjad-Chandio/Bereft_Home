using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenuController : MonoBehaviour
{
    public GameObject pauseMenuPanel;
    public GameObject hidingPanel;
    public AudioSource[] audioSources;

    private bool isPaused = false;

    void Start()
    {
        pauseMenuPanel.SetActive(false);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.Space))
        {
            if (isPaused)
                ResumeGame();
            else
                PauseGame();
        }
    }

    public void PauseGame()
    {
        isPaused = true;
        pauseMenuPanel.SetActive(true);

        ToggleAudioSources(false);
        hidingPanel.SetActive(false);

        Time.timeScale = 0f;

        SetCursorState(true);
    }

    public void ResumeGame()
    {
        isPaused = false;
        pauseMenuPanel.SetActive(false);

        ToggleAudioSources(true);
        hidingPanel.SetActive(true);

        Time.timeScale = 1f;

       
        SetCursorState(false);
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

    
    private void SetCursorState(bool isCursorVisible)
    {
        Cursor.lockState = isCursorVisible ? CursorLockMode.None : CursorLockMode.Locked;
        Cursor.visible = isCursorVisible;
    }

  
    private void ToggleAudioSources(bool play)
    {
        foreach (AudioSource audio in audioSources)
        {
            if (play)
                audio.UnPause();
            else
                audio.Pause();
        }
    }
}
