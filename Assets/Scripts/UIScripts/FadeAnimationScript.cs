using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeAnimationScript : MonoBehaviour
{
    Animator fadeAnimator;
    public float fadeCooldown = 4;
    public bool Fade { get; set; }

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

        while (currentFadeCooldown < fadeCooldown)
        {
            currentFadeCooldown += Time.deltaTime;
            yield return null;
        }

        fadeAnimator.SetBool("Fade", false);
    }


}
