using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class FadeAnimationScript : MonoBehaviour
{
    Animator fadeAnimator;
    public float fadeCooldown = 1;
    public float animationTransitionTime = 0.75f;
    public bool Fade { get; set; }

    public static UnityAction OnFaded = delegate { };

    private float currentFadeCooldown = 0;

    // Start is called before the first frame update
    void Start()
    {
        fadeAnimator = GetComponent<Animator>();
        StartFade(0);
    }

    public void StartFade(float fadeDuration)
    {
        StartCoroutine(StartFadeTransition(fadeDuration));
    }

    public void SetFade(bool value)
    {
        fadeAnimator.SetBool("Fade", value);
    }

    IEnumerator StartFadeTransition(float fadeDuration)
    {
        fadeAnimator.SetBool("Fade", true);

        yield return new WaitForSeconds(animationTransitionTime);
        //yield return new WaitUntil(() => (fadeAnimator.GetCurrentAnimatorStateInfo(0).tra > 1));          

        while (currentFadeCooldown < fadeCooldown)
        {
            currentFadeCooldown += Time.deltaTime;
            yield return null;
        }

        OnFaded?.Invoke();

        yield return new WaitForSeconds(fadeDuration);

        Debug.Log("Is Faded");

        fadeAnimator.SetBool("Fade", false);
        OnFaded = delegate { };
    }


}
