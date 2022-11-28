using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class PauseManager : MonoBehaviour
{
    public bool CanPause { get; set; } = false;

    public UnityEvent OnPause;
    public UnityEvent OnUnpause;

    public KeyCode pauseKeycode = KeyCode.Escape;
    public KeyCode secondaryPauseKeycode = KeyCode.P;
    public GameObject childObject;

    private static bool isPaused = false;

    private void Awake()
    {
        childObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (!CanPause)
            return;

	    if ((Input.GetKeyDown(pauseKeycode) || Input.GetKeyDown(secondaryPauseKeycode)))
		    PauseResponse();

    }

    public void PauseResponse()
    {
        isPaused = !isPaused;

        if (isPaused)
        {
            OnPause?.Invoke();
            childObject.SetActive(true);            
            Time.timeScale = 0;
        }
        else
        {
            OnUnpause?.Invoke();
            childObject.SetActive(false);
            Time.timeScale = 1;
        }
    }

    public void ResumeButton()
    {
        PauseResponse();
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(0);
        Time.timeScale = 1;
    }

    public void QuitGame()
    {
        Application.Quit();
    }

}
