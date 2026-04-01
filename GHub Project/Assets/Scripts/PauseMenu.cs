using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;

public class PauseMenu : MonoBehaviour
{
    [Header("UI")]
    public GameObject pausePanel;

    [Header("First Selected Button")]
    public GameObject resumeButton; // Assign your Resume button here

    [Header("Input")]
    public InputActionReference pauseAction;

    private bool isPaused = false;

    void OnEnable()
    {
        if (pauseAction != null)
        {
            pauseAction.action.Enable();
            pauseAction.action.performed += OnPauseInput;
        }
    }

    void OnDisable()
    {
        if (pauseAction != null)
        {
            pauseAction.action.performed -= OnPauseInput;
            pauseAction.action.Disable();
        }
    }

    void Start()
    {
        if (pausePanel != null)
            pausePanel.SetActive(false);
        Time.timeScale = 1f;
    }

    private void OnPauseInput(InputAction.CallbackContext context)
    {
        if (isPaused)
            ResumeGame();
        else
            PauseGame();
    }

    public void PauseGame()
    {
        if (pausePanel != null)
            pausePanel.SetActive(true);

        Time.timeScale = 0f;
        isPaused = true;

        // IMPORTANT: Select the resume button for joystick control
        StartCoroutine(SelectButtonNextFrame());
    }

    private System.Collections.IEnumerator SelectButtonNextFrame()
    {
        yield return null; // Wait one frame

        if (resumeButton != null && EventSystem.current != null)
        {
            EventSystem.current.SetSelectedGameObject(null);
            EventSystem.current.SetSelectedGameObject(resumeButton);
        }
    }

    public void ResumeGame()
    {
        if (pausePanel != null)
            pausePanel.SetActive(false);

        Time.timeScale = 1f;
        isPaused = false;

        if (EventSystem.current != null)
            EventSystem.current.SetSelectedGameObject(null);
    }

    public void QuitGame()
    {
        Time.timeScale = 1f;

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}