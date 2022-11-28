using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class MainMenuScript : MonoBehaviour
{
    public UnityEvent OnScreenFade;

    private void OnDisable()
    {
        FadeAnimationScript.OnFaded -= (ScreenFadeEventCall);
    }

    public void StartGame()
    {
        FadeAnimationScript.OnFaded += (ScreenFadeEventCall);
        //Debug.Log("Subscribe to fade event");
    }

    public void ScreenFadeEventCall()
    {
        OnScreenFade?.Invoke();
        //Debug.Log("main menu screen fade");
    }

    public void QuitGame()
    {
        Application.Quit();
    }

}
