using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

public class PauseMenuController : MonoBehaviour
{
    [Header("UI Panels")]
    [SerializeField] private GameObject pauseMenuCanvas;
    [SerializeField] private GameObject optionsMenuCanvas;

    [Header("Scene Navigation")]
    [SerializeField] private string mainMenuSceneName = "MainMenu";

    private PlayerInput playerInput;
    private InputAction pauseAction;
    private bool isPaused = false;

    private void Awake()
    {
        playerInput = GetComponent<PlayerInput>();
        if (playerInput != null)
        {
            pauseAction = playerInput.actions["Pause"];
        }
    }

    private void OnEnable()
    {
        if (pauseAction != null)
        {
            pauseAction.performed += TogglePauseFromInput;
        }
    }

    private void OnDisable()
    {
        if (pauseAction != null)
        {
            pauseAction.performed -= TogglePauseFromInput;
        }
    }

    private void TogglePauseFromInput(InputAction.CallbackContext context)
    {
        if (optionsMenuCanvas != null && optionsMenuCanvas.activeSelf)
        {
            CloseOptions();
            return;
        }

        if (isPaused)
        {
            ResumeGame();
        }
        else
        {
            PauseGame();
        }
    }

    public void PauseGame()
    {
        isPaused = true;
        Time.timeScale = 0f;
        if (pauseMenuCanvas != null) pauseMenuCanvas.SetActive(true);
    }

    public void ResumeGame()
    {
        isPaused = false;
        Time.timeScale = 1f;
        if (pauseMenuCanvas != null) pauseMenuCanvas.SetActive(false);
        if (optionsMenuCanvas != null) optionsMenuCanvas.SetActive(false);
    }

    public void OpenOptions()
    {
        if (pauseMenuCanvas != null) pauseMenuCanvas.SetActive(false);
        if (optionsMenuCanvas != null) optionsMenuCanvas.SetActive(true);
    }

    public void CloseOptions()
    {
        if (optionsMenuCanvas != null) optionsMenuCanvas.SetActive(false);
        if (pauseMenuCanvas != null) pauseMenuCanvas.SetActive(true);
    }

    public void ExitToMainMenu()
    {
        Time.timeScale = 1f;
        if (!string.IsNullOrEmpty(mainMenuSceneName))
        {
            SceneManager.LoadScene(mainMenuSceneName);
        }
    }
}