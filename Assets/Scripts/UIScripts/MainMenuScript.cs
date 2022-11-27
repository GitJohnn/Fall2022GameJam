using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class MainMenuScript : MonoBehaviour
{
    public UnityEvent OnScreenFade;

    private void OnDisable()
    {
        FadeAnimationScript.OnFade -= ScreenFadeEventCall;
    }

    public void StartGame()
    {
        FadeAnimationScript.OnFade += ScreenFadeEventCall;
    }

    public void ScreenFadeEventCall()
    {
        OnScreenFade?.Invoke();
    }

    public void QuitGame()
    {
        Application.Quit();
    }

}
