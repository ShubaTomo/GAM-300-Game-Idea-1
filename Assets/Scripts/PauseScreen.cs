using UnityEngine;

public class PauseScreen : MonoBehaviour
{
    [Header("References")]
    public GameObject pausePanel;
    public GameObject gameplayHUD;

    private bool isPaused;

    private void Start()
    {
        pausePanel.SetActive(false);

        if (gameplayHUD != null)
            gameplayHUD.SetActive(true);

        Time.timeScale = 1f;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused)
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }
    }

    public void Pause()
    {
        pausePanel.SetActive(true);

        if (gameplayHUD != null)
            gameplayHUD.SetActive(false);

        Time.timeScale = 0f;

        Cursor.lockState =
            CursorLockMode.None;

        Cursor.visible = true;

        isPaused = true;
    }

    public void Resume()
    {
        pausePanel.SetActive(false);

        if (gameplayHUD != null)
            gameplayHUD.SetActive(true);

        Time.timeScale = 1f;

        Cursor.lockState =
            CursorLockMode.Locked;

        Cursor.visible = false;

        isPaused = false;
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}