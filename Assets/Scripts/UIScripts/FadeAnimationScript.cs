using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class FadeAnimationScript : MonoBehaviour
{
    Animator fadeAnimator;
    public float fadeCooldown = 4;
    public float animationTransitionTime = 0.75f;
    public bool Fade { get; set; }

    public static UnityAction OnFade = delegate { };

    private float currentFadeCooldown = 0;

    // Start is called before the first frame update
    void Start()
    {
        fadeAnimator = GetComponent<Animator>();
        StartFade();
    }

    public void StartFade()
    {
        StartCoroutine(StartFadeTransition());
    }

    IEnumerator StartFadeTransition()
    {
        fadeAnimator.SetBool("Fade", true);

        yield return new WaitForSeconds(animationTransitionTime);
        //yield return new WaitUntil(() => (fadeAnimator.GetCurrentAnimatorStateInfo(0).tra > 1));          

        while (currentFadeCooldown < fadeCooldown)
        {
            currentFadeCooldown += Time.deltaTime;
            yield return null;
        }

        OnFade();
        Debug.Log("Is Faded");

        fadeAnimator.SetBool("Fade", false);
    }


}
