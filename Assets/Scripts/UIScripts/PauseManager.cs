using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PauseManager : MonoBehaviour
{
    public bool CanPause { get; set; } = false;

    public UnityEvent OnPause;
    public UnityEvent OnUnpause;

    public KeyCode pauseKeycode = KeyCode.Escape;
    public KeyCode secondaryPauseKeycode = KeyCode.P;
    public GameObject childObject;

    private static bool isPaused = false;

    // Update is called once per frame
    void Update()
    {
        if (isPaused)
            childObject.SetActive(true);
        else
            childObject.SetActive(false);

        if (!CanPause)
            return;

        if ((Input.GetKeyDown(pauseKeycode) || Input.GetKeyDown(secondaryPauseKeycode)) && !isPaused)
            PauseResponse();

    }

    public void PauseResponse()
    {
        isPaused = !isPaused;

        if (isPaused)
        {
            OnPause?.Invoke();
            Time.timeScale = 0;
        }
        else
        {
            OnUnpause?.Invoke();
            Time.timeScale = 1;
        }
    }

    public void ResumeButton()
    {
        PauseResponse();
    }

    public void RestartGame()
    {

    }

    public void QuitGame()
    {
        Application.Quit();
    }

}
